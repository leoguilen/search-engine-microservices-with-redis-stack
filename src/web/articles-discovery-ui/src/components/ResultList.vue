<template>
  <div v-infinite-scroll="() => this.$emit(`end-of-list`)" :infinite-scroll-disabled="disableInfiniteScroll"
    infinite-scroll-distance="100" infinite-scroll-throttle-delay="500">
    <div style="background-color: transparent" class="relative flex flex-col px-4 pt-5 pb-12 font-sans text-gray-700 bg-gray-200 sm:px-6 lg:px-8">
      <div class="flex-1 space-y-8">
        <div style="top: calc(1rem * 1); width: 100vh;" v-for="(article, id) of results" :key="id" class="sticky w-full max-w-xl px-8 py-12 mx-auto space-y-4 bg-white border rounded-lg shadow-lg">
          <h2 class="space-y-1 text-2xl font-bold leading-none text-gray-900">
            <span class="block">
              {{ article.title }}
            </span>
          </h2>
          <span>- <i>{{article.author}}</i></span>
          <p>
            {{ article.summary}}
          </p>
          <div class="flex justify-end">
            <a :href="article.link" target="_blank"
             >Read more</a> 
          </div>
        </div>
      </div>
    </div>
    <div class="flex flex-col w-full overflow-auto">
      <div class="flex flex-row w-full mb-2">
        <div
          :class="`w-full md:w-1/2 p-4 text-center mb-4 ${message.level === `warning` ? `bg-orange-500 dark:bg-orange-800 text-white` : message.level === `error` ? `bg-red-600 dark:bg-red-800 text-white` : ``} rounded-md font-bold`">
          <span>
            {{  message.text  }}
          </span>
          <span v-if="results.length === 0">
            No items were found with the searched value.
          </span>
          <br>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: `ResultList`,
  components: {
  },
  props: {
    results: {
      type: Array,
      required: true,
    },
    pageSize: {
      type: Number,
      default: function () {
        return 10;
      }
    },
    message: {
      type: Object,
      default: function () {
        return {
          text: ``,
          level: `normal`,
        };
      }
    },
    disableInfiniteScroll: {
      type: Boolean,
      default: function () {
        false;
      }
    },
    lowestPage: {
      type: Number,
      default: function () {
        return 1;
      }
    },
    scrollToInitialPage: {
      type: Number,
      default: function () {
        return 1;
      }
    },
  },
  data: function () {
    return {
    }
  },
  methods: {
  },
}
</script>
