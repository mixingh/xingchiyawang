import Vue from 'vue'
import VueRouter from 'vue-router'

import Login from '@/components/Login.vue'
import Home from '@/views/Home/Home.vue'
import Index from '@/components/Index.vue'
import pathArr from '@/router/pathArr.js'

import CustomBasicInfo from '@/views/custom/info/CustomBasicInfo.vue'
import ContractsBasicInfo from '@/views/custom/contacts/CotractsBasicInfo.vue'
import ContractInfo from '@/views/custom/contract/ContractInfo.vue'
import ContracterInfo from '@/views/custom/contract/ContracterInfo.vue'

import ActionNotes from '@/views/customrelation/ActionNotes.vue'
import ActionPlan from '@/views/customrelation/ActionPlan.vue'
import AdviceHandle from '@/views/customrelation/AdviceHandle.vue'

import SalesGroups from '@/views/salesmen/SalesGroups.vue'
import SalesBasicInfo from '@/views/salesmen/SalesBasicInfo.vue'
import Achievement from '@/views/salesmen/Achievement.vue'

import Roles from '@/views/rights/Roles.vue'
import Users from '@/views/rights/Users.vue'

Vue.use(VueRouter)

const router = new VueRouter({
  routes: [
    { path: '/', redirect: '/login' },
    { path: '/login', component: Login },
    {
      path: '/home',
      component: Home,
      redirect: '/home/index',
      children: [
        { path: 'index', component: Index },
        { path: 'custombasicinfo', component: CustomBasicInfo },
        { path: 'contractsbasicinfo', component: ContractsBasicInfo },
        { path: 'contractinfo', component: ContractInfo },
        { path: 'contracterinfo', component: ContracterInfo },
        { path: 'actionnotes', component: ActionNotes },
        { path: 'actionplan', component: ActionPlan },
        { path: 'advicehandle', component: AdviceHandle },
        { path: 'salesgroups', component: SalesGroups },
        { path: 'salesbasicinfo', component: SalesBasicInfo },
        { path: 'achievement', component: Achievement },
        { path: 'roles', component: Roles },
        { path: 'users', component: Users }
      ]
    }
  ]
})

// 全局前置守卫
// 只要发生了路由跳转，必然会触发 beforeEach 指定的 function 回调函数
router.beforeEach(function(to, from, next) {
  // 获取 router/pathArr.js 中的数组，如果要访问的地址在数组中，则判断是否具有权限访问
  // 重点： 每次新添加一个需要访问权限页面时，都要在 router/pathArr.js 中添加新页面的地址
  if (pathArr.indexOf(to.path) !== -1) {
    const token = window.sessionStorage.getItem('token')
    if (token) {
      // 放行
      next()
    } else {
      // 跳转到登录界面
      next('/login')
    }
  } else {
    // 如果访问地址不在 pathArr 数组中，直接放行
    next()
  }
})

export default router
