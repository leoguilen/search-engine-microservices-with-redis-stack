<template>
  <div class="text-black dark:text-gray-100">
    <form class="relative flex flex-col" method="" action="#" @submit.prevent="$emit('search')">
      <input ref="searchField"
        :class="`w-full h-full bg-white dark:bg-gray-700 p-4 pr-24 sm:pr-56 text-left placeholder-gray-700 dark:placeholder-gray-400 border-2 border-gray-600 rounded-xl outline-none focus:border-green-400`"
        :placeholder="placeholder" type="search" :value="value" name="Articles Discovery Search"
        @input="loadSuggestions" @keydown.enter="$emit(`search`)">
      <div class="absolute top-0 right-0 flex flex-row-reverse pr-3">
        <svg
          class="w-8 h-8 p-1 mx-1 my-3 text-gray-900 cursor-pointer stroke-current stroke-2 dark:text-gray-200 hover:text-green-400"
          xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" @click="$emit(`search`)">
          <title>Search</title>
          <circle cx="10" cy="10" r="7" />
          <line x1="21" y1="21" x2="15" y2="15" />
        </svg>
      </div>
    </form>
  </div>
</template>

<script>
export default {
  name: `SearchField`,
  components: {

  },
  props: {
    value: {
      type: String,
      default: function () {
        return ``;
      }
    },
    placeholder: {
      type: String,
      default: function () {
        return `Search in all open directories...`;
      }
    },
    focus: {
      type: Boolean,
      default: function () {
        return false;
      }
    },
  },
  data: () => {
    return {
      suggestions: [],
    };
  },
  methods: {
    loadSuggestions(event){
      var inputValue = event.target.value;
      this.$emit('input', inputValue)
      if(inputValue.length > 3){
        this.$http.get(`articles?q=${inputValue}&page=0&itemsPerPage=10`).then(res => {
          this.suggestions = res.body.suggestedArticles;
          console.log(this.suggestions)
        }).catch(err => console.error(err));
      }
    }
  },
}

</script>