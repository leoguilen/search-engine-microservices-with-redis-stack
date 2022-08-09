package main

import (
	"context"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
	"strings"

	"github.com/go-redis/redis"
	"github.com/gocolly/colly"
)

var (
	baseArticlesLinkFormat string = "https://www.discovery.com/space/all-space-articles/p/%d"
	pageNumber             int    = 1
	articlesPerPage        int    = 20
	redisClient                   = redis.NewClient(&redis.Options{
		Addr:       "localhost:6379",
		Password:   "", // no password set
		DB:         0,
		MaxRetries: 1,
		OnConnect:  redisConnectionCheckHook,
	})
)

type InvokeResponse struct {
	Outputs     map[string]interface{}
	Logs        []string
	ReturnValue string
}

type Article struct {
	Title       string `json:"title"`
	Summary     string `json:"summary"`
	Content     string `json:"content"`
	Author      string `json:"author"`
	PublishedAt string `json:"published_at"`
	Link        string `json:"link"`
}

func redisConnectionCheckHook(cn *redis.Conn) error {
	if err := cn.Ping().Err(); err != nil {
		log.Panicf("Error on connecting to redis: %v", err)
		return err
	}
	log.Print("Connected to Redis")
	return nil
}

func publishExtractedArticleEvent(ctx context.Context, a *Article) {
	var articleMap map[string]interface{}
	data, err := json.Marshal(a)
	if err != nil {
		log.Panic(err)
	}
	json.Unmarshal(data, &articleMap)

	log.Printf("Publishing event to Redis: %v", articleMap)
	if err := redisClient.XAdd(&redis.XAddArgs{
		Stream:       "extracted-articles-stream",
		MaxLen:       0,
		MaxLenApprox: 0,
		ID:           "",
		Values:       articleMap,
	}).Err(); err != nil {
		log.Panicf("Error on publishing event to Redis: %v", err)
	}
}

func extractArticles(ctx context.Context) ([]Article, error) {
	collector := colly.NewCollector(
		colly.AllowedDomains("discovery.com", "www.discovery.com"),
		colly.Async(true),
	)

	innerCollector := collector.Clone()

	articles := make([]Article, 0, articlesPerPage)

	collector.OnError(func(r *colly.Response, err error) {
		log.Println("Request URL:", r.Request.URL, "failed with response:", r, "\nError:", err)
	})

	collector.OnRequest(func(r *colly.Request) {
		log.Println("Visiting", r.URL.String())
	})

	collector.OnHTML(`h4[class=m-MediaBlock__a-Headline]`, func(e *colly.HTMLElement) {
		link := e.ChildAttr("a[href]", "href")
		if !strings.Contains(link, "/space/") {
			return
		}
		log.Printf("Link found: %q -> %s\n", strings.TrimSpace(e.Text), link)
		innerCollector.Visit(link)
	})

	innerCollector.OnError(func(r *colly.Response, err error) {
		log.Println("Request URL:", r.Request.URL, "failed with response:", r, "\nError:", err)
	})

	innerCollector.OnRequest(func(r *colly.Request) {
		log.Println("Visiting", r.URL.String())
	})

	innerCollector.OnHTML(`article[class=article-content]`, func(e *colly.HTMLElement) {
		article := Article{
			Title:       strings.TrimSpace(e.ChildText(`h1[class=o-AssetTitle__a-Headline]`)),
			Author:      strings.TrimSpace(e.ChildText(`span[class=o-Attribution__a-Author--Prefix]`)),
			PublishedAt: strings.TrimSpace(e.ChildText(`div[class=o-AssetPublishDate]`)),
			Summary:     strings.TrimSpace(e.ChildText(`div[class=o-AssetDescription__a-Description]`)),
			Content:     strings.TrimSpace(e.ChildText(`section[class="o-CustomRTE"]`)),
			Link:        e.Request.URL.String(),
		}
		articles = append(articles, article)
		go publishExtractedArticleEvent(ctx, &article)
	})

	if err := collector.Visit(fmt.Sprintf(baseArticlesLinkFormat, pageNumber)); err != nil {
		return nil, err
	}
	collector.Wait()
	innerCollector.Wait()
	return articles, nil
}

func handler(w http.ResponseWriter, r *http.Request) {
	a, err := extractArticles(r.Context())
	if err != nil {
		log.Panic(err)
	}

	log.Printf("Articles found: %v", a)
	if len(a) > 0 {
		pageNumber++
	}

	invokeResponse := InvokeResponse{Logs: []string{}, ReturnValue: ""}
	js, err := json.Marshal(invokeResponse)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.Write(js)
}

func main() {
	customHandlerPort, exists := os.LookupEnv("FUNCTIONS_CUSTOMHANDLER_PORT")
	if !exists {
		customHandlerPort = "8080"
	}

	mux := http.NewServeMux()
	mux.HandleFunc("/extractorWorker", handler)
	log.Println("Go server Listening on: ", customHandlerPort)
	log.Fatal(http.ListenAndServe(":"+customHandlerPort, mux))
}
