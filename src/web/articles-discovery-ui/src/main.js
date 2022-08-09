import Vue from 'vue'
import App from './App.vue'
import router from './router'
import VueResource from 'vue-resource';
import "@/assets/css/tailwind.css";

Vue.use(VueResource);

Vue.http.options.root = 'http://localhost:7071/api'
Vue.config.productionTip = false

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
