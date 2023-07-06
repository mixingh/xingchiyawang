import Vue from 'vue'
import App from './App.vue'
import router from './router'
import './assets/css/global.css'

// 导入 Element-UI 组件库
import 'element-ui/lib/theme-chalk/index.css'
import {
  Button, Input, Form, FormItem, Tooltip,
  Container, Header, Aside, Main, Footer,
  Menu, Submenu, MenuItemGroup, MenuItem,
  Badge, Breadcrumb, BreadcrumbItem, Option,
  Card, Row, Col, Table, TableColumn,
  Pagination, Dialog, MessageBox, Descriptions,
  DescriptionsItem, Tag, Backtop, Icon, Divider,
  Select, DatePicker, Image, Upload, Tabs, TabPane,
  Alert, RadioGroup, Radio, Dropdown, DropdownMenu,
  DropdownItem, Avatar, Calendar, Autocomplete
} from 'element-ui'

// 导入方法
import OnlyShowOnceMsg from '@/utils/showMsgOnly.js'

Vue.use(Button)
Vue.use(Input)
Vue.use(Form)
Vue.use(FormItem)
Vue.use(Tooltip)
Vue.use(Container)
Vue.use(Header)
Vue.use(Aside)
Vue.use(Main)
Vue.use(Footer)
Vue.use(Menu)
Vue.use(Submenu)
Vue.use(MenuItemGroup)
Vue.use(MenuItem)
Vue.use(Badge)
Vue.use(Breadcrumb)
Vue.use(BreadcrumbItem)
Vue.use(Option)
Vue.use(Card)
Vue.use(Row)
Vue.use(Col)
Vue.use(Table)
Vue.use(TableColumn)
Vue.use(Pagination)
Vue.use(Dialog)
Vue.use(Descriptions)
Vue.use(DescriptionsItem)
Vue.use(Tag)
Vue.use(Backtop)
Vue.use(Icon)
Vue.use(Divider)
Vue.use(Select)
Vue.use(DatePicker)
Vue.use(Image)
Vue.use(Upload)
Vue.use(Tabs)
Vue.use(TabPane)
Vue.use(Alert)
Vue.use(RadioGroup)
Vue.use(Radio)
Vue.use(Dropdown)
Vue.use(DropdownMenu)
Vue.use(DropdownItem)
Vue.use(Avatar)
Vue.use(Calendar)
Vue.use(Autocomplete)
Vue.prototype.$message = OnlyShowOnceMsg
Vue.prototype.$confirm = MessageBox.confirm

Vue.config.productionTip = false

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
