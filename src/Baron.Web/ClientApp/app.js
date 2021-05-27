import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/root/app-root'
import { FontAwesomeIcon } from './icons'
import PageHead from 'components/shared/page-head';
import Notifications from 'vue-notification';
import VueContentPlaceholders from "vue-content-placeholders"
import VueApexCharts from 'vue-apexcharts'
import BIText from 'components/input/text';
import BVMonitorStatus from 'components/shared/monitor-status';
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.use(VueApexCharts)
Vue.use(BootstrapVue)

Vue.use(VueContentPlaceholders)
Vue.use(require('vue-moment'));

Vue.use(Notifications);

// Registration of global components
Vue.component('apexchart', VueApexCharts)
Vue.component('icon', FontAwesomeIcon);
Vue.component('page-head', PageHead);
Vue.component('bi-text', BIText);
Vue.component('monitor-status', BVMonitorStatus);


Vue.prototype.$http = axios

sync(store, router)

const app = new Vue({
  store,
  router,
  ...App
})

export {
  app,
  router,
  store
}
