<template>
  <div class="w-full h-full p-4">
    <div class="flex flex-row justify-between w-full h-auto font-semibold md:p-4 md:absolute md:top-0 md:left-0">
      <a class="text-lg hover:text-green-400" href="https://github.com/leoguilen/search-engine-microservices-with-redis-stack" target="_blank">
        Source Code
      </a>
    </div>
    <div class="w-full h-full">
      <div class="flex flex-col justify-center flex-shrink-0 w-full md:h-full">
        <LogoHeader class="md:-mt-10" :showDescription="true" />
        <SearchField class="w-full max-w-3xl mx-auto mt-2 mb-4 md:my-4" v-model="searchQuery" :focus="true"
          :placeholder="'Search for various article links here...'"
          @search="
            searchQuery.length > 0 ?
              $router.push({
                name: `Search`,
                query: {
                  q: searchQuery
                }
              }) :
              false
          " />
      </div>
    </div>
  </div>
</template>

<script>
import SearchField from '@/components/SearchField';
import LogoHeader from '@/components/LogoHeader';

export default {
  name: `Home`,
  components: {
    SearchField,
    LogoHeader,
  },
  data: function () {
    return {
      searchQuery: "",
    };
  },
}
</script>
