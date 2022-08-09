<template>
  <div class="w-full h-full p-4">
    <vue-headful :title="pageTitle" />
    <div class="flex-row justify-start md:flex md:mb-4">
      <div class="flex-shrink-0 text-center md:w-20 md:text-left">
        <router-link class="hover:text-green-400" :to="{
          name: `Home`
        }">
          <h1 class="text-3xl font-semibold cursor-pointer md:py-1">
            <span class="hidden md:inline">ART</span>
            <span class="inline md:hidden">Articles Discovery</span>
          </h1>
        </router-link>
      </div>
      <SearchField class="h-auto ml-0 md:w-3/5 lg:w-192" v-model="searchQuery"
        v-observe-visibility="handleVisibilityChanged" :focus="false"
        :placeholder="`Search ${results.length} articles found...`" @search="search(searchQuery)" />
    </div>

    <ResultList 
      class="flex flex-row justify-start w-full h-auto pb-16" 
      :results="results"
      :message="message"
      @message-button="handleMessageButton($event)" />
  </div>
</template>

<script>
import SearchField from '@/components/SearchField';
import ResultList from '@/components/ResultList';

export default {
  name: `Search`,
  components: {
    SearchField,
    ResultList,
  },
  data: function () {
    return {
      results: [],
      suggestions: [],
      searchQuery: "",
      message: {
        text: "",
        level: "normal",
      },
    };
  },
  methods: {
    async search(query) {
      this.message = {
        text: `Loading Results...`,
        level: `normal`,
      }
  
      try {
        this.$http.get(`articles?q=${query}&page=0&itemsPerPage=10`).then(res => {
          this.results = res.body.matchedArticles;
          this.suggestions = res.body.suggestedArticles;
        }).catch(err => {
          console.error(err);
          this.message = {
            text: `Couldn't load search results :/`,
            level: `error`,
          }
        });
      }
      catch(err) {
        this.message = {
          text: `Couldn't load search results :/`,
          level: `error`,
        }
      }
      finally{
        this.message = {
          text: ``,
          level: `normal`,
        }
      }
    },
  },
  mounted() {
    const inputValue = this.$route.query.q;
    this.searchQuery = inputValue;
    if (inputValue) {
      this.search(inputValue);
    }
  },
}
</script>
