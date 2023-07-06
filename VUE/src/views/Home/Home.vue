<template>
  <el-container class="home-container">
    <!-- 头部区域 -->
    <el-header>
      <div>
        <span>客户关系管理系统</span>
      </div>
      <div>
        <!-- 刷新按钮 -->
        <el-button icon="el-icon-refresh" title="刷新" @click="updatePage" circle></el-button>
        <!-- 注销按钮 -->
        <el-button icon="el-icon-switch-button" title="注销" @click="logout" circle></el-button>
      </div>
    </el-header>
    <el-container>
      <!-- 侧边导航区域 -->
      <Aside></Aside>
      <el-container>
        <!-- 中部主体区域 -->
        <el-main class="el-container">
          <!-- 占位符 -->
          <router-view></router-view>
        </el-main>
      </el-container>
    </el-container>
  </el-container>
</template>

<script>
import Aside from '@/components/subcomponents/Aside.vue'

export default {
  name: 'MyHome',
  methods: {
    // 注销
    async logout() {
      const confirmText = await this.$confirm('您确定要退出系统？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).catch(err => err)
      if (confirmText !== 'confirm') return
      // 清空 token
      window.sessionStorage.clear()
      // 跳转到登录页面
      this.$router.replace('/login')
    },
    // 刷新页面
    updatePage() {
      this.reload()
    }
  },
  components: {
    Aside
  },
  inject: ['reload']
}
</script>

<style lang="less" scoped>
.router-link-active {
  color: #fff;
  text-decoration: none;
}
.home-container {
  height: 100%;
}
.el-header {
  background-color: #409eff;
  display: flex;
  justify-content: space-between;
  padding-left: 0;
  align-items: center;
  color: #fff;
  font-size: 20px;
  > div {
    display: flex;
    align-items: center;
    span {
      margin-left: 15px;
    }
    img {
      height: 60px;
    }
  }
  .item {
    border: none;
    margin-right: 40px;
  }
  .itemnum {
    border: none;
    margin-right: 40px;
  }
  .el-button {
    margin-left: 30px;
  }
}
.el-main {
  background-color: rgb(236, 245, 255);
}
.el-aside {
  background-color: rgb(217, 236, 255);
}
</style>
