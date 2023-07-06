<template>
  <div class="login-container">
    <!-- 登录盒子 -->
    <div class="login-box">
      <!-- 头部图片部分 -->
      <div class="login-pic">
        <img src="../assets/maomi.jpg" alt="" />
      </div>
      <!-- 角色选择 -->
      <div class="login-user">
        <el-select v-model="value">
          <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value"> </el-option>
        </el-select>
      </div>
      <!-- 登录表单部分 -->
      <el-form :model="loginForm" :rules="loginRules" ref="loginFormRef" class="login-form">
        <!-- 输入框部分 -->
        <el-form-item prop="username">
          <el-input placeholder="请输入账号" prefix-icon="el-icon-user" v-model="loginForm.username"> </el-input>
        </el-form-item>
        <el-form-item prop="password">
          <el-input placeholder="请输入密码" show-password prefix-icon="el-icon-lock" v-model="loginForm.password"> </el-input>
        </el-form-item>
        <div class="drag" ref="dragDiv">
          <div class="drag_bg"></div>
          <div class="drag_text">{{ confirmWords }}</div>
          <div
            ref="moveDiv"
            @mousedown="mousedownFn($event)"
            :class="{ handler_ok_bg: confirmSuccess }"
            class="handler handler_bg"
            style="position: absolute; top: 0px; left: 0px; border-radius: 0px 40px 40px 0px; border: medium none; height: 34px;"
          ></div>
        </div>
        <!-- 提交按钮部分 -->
        <el-form-item class="mybtn">
          <el-button style="width: 400px" @click="login" round :plain="true" :disabled="loginEnabled">登录</el-button>
        </el-form-item>
      </el-form>
    </div>
    <div class="bottom">
      Powered by Xingheng
    </div>
  </div>
</template>

<script>
import request from '@/utils/request.js'

export default {
  name: 'MyLogin',
  data() {
    return {
      loginForm: {
        username: '',
        password: ''
      },
      // 这是验证表单的规则
      loginRules: {
        // 验证账号是否合法
        username: [
          { required: true, message: '请输入账号', trigger: 'blur' },
          { min: 3, message: '长度在 3 个字符以上', trigger: 'blur' }
        ],
        // 验证密码是否合法
        password: [{ required: true, message: '请输入密码', trigger: 'blur' }]
      },
      options: [
        {
          value: 'admin',
          label: '管理员'
        },
        {
          value: 'sale',
          label: '销售员'
        }
      ],
      value: 'admin',
      beginClientX: 0 /* 距离屏幕左端距离 */,
      mouseMoveStata: false /* 触发拖动状态  判断 */,
      maxwidth: '' /* 拖动最大宽度，依据滑块宽度算出来的 */,
      confirmWords: '拖动滑块验证' /* 滑块文字 */,
      confirmSuccess: false /* 验证成功判断 */,
      loginEnabled: true
    }
  },
  methods: {
    mousedownFn(e) {
      if (!this.confirmSuccess) {
        e.preventDefault && e.preventDefault() // 阻止文字选中等 浏览器默认事件
        this.mouseMoveStata = true
        this.beginClientX = e.clientX
      }
    }, // mousedoen 事件
    successFunction() {
      this.confirmSuccess = true
      this.confirmWords = '验证通过'
      this.loginEnabled = false
      if (window.addEventListener) {
        document.getElementsByTagName('html')[0].removeEventListener('mousemove', this.mouseMoveFn)
        document.getElementsByTagName('html')[0].removeEventListener('mouseup', this.moseUpFn)
      } else {
        document.getElementsByTagName('html')[0].removeEventListener('mouseup', () => {})
      }
      document.getElementsByClassName('drag_text')[0].style.color = '#fff'
      document.getElementsByClassName('handler')[0].style.left = this.maxwidth + 'px'
      document.getElementsByClassName('drag_bg')[0].style.width = this.maxwidth + 'px'
    },
    // 验证成功函数
    mouseMoveFn(e) {
      if (this.mouseMoveStata) {
        let width = e.clientX - this.beginClientX
        if (width > 0 && width <= this.maxwidth) {
          document.getElementsByClassName('handler')[0].style.left = width + 'px'
          document.getElementsByClassName('drag_bg')[0].style.width = width + 'px'
        } else if (width > this.maxwidth) {
          this.successFunction()
        }
      }
    }, // mousemove事件
    moseUpFn(e) {
      this.mouseMoveStata = false
      var width = e.clientX - this.beginClientX
      if (width < this.maxwidth) {
        document.getElementsByClassName('handler')[0].style.left = 0 + 'px'
        document.getElementsByClassName('drag_bg')[0].style.width = 0 + 'px'
      }
    }, // mouseup事件
    login() {
      this.$refs.loginFormRef.validate(async valid => {
        if (!valid) return
        // 判断账号密码是否正确
        if (this.value === 'admin') {
          const params = {
            adminId: this.loginForm.username,
            adminPwd: this.loginForm.password
          }
          const { data: res } = await request.post('admins/AdminLogin', params)
          if (res.status !== 200) return this.$message.error('账号或密码错误')
          // 保存 token 值，用于判断身份、登录状态等
          window.sessionStorage.setItem('token', 'Bearer manager')
          this.$router.replace('/home')
          this.$message.success('登录成功')
        } else {
          const params = {
            salesManId: this.loginForm.username,
            salesManpwd: this.loginForm.password
          }
          const { data: res } = await request.post('Salesmen/SalesLogin', params)
          if (res.status !== 200) return this.$message.error('账号或密码错误')
          window.sessionStorage.setItem('token', 'Bearer user')
          this.$router.replace('/home')
          this.$message.success('登录成功')
        }
      })
    }
  },
  mounted() {
    this.maxwidth = this.$refs.dragDiv.clientWidth - this.$refs.moveDiv.clientWidth
    document.getElementsByTagName('html')[0].addEventListener('mousemove', this.mouseMoveFn)
    document.getElementsByTagName('html')[0].addEventListener('mouseup', this.moseUpFn)
  }
}
</script>

<style lang="less" scoped>
.login-container {
  background-image: linear-gradient(to right bottom, #409eff, #62adff, #7fbcff, #9acbff, #b5d9ff, #c3e0ff, #d0e7ff, #deeeff, #e2f0ff, #e5f1ff, #e9f3ff, #ecf5ff);
  height: 100%;
}
.login-box {
  width: 450px;
  height: 320px;
  border-radius: 5px;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  box-shadow: 0 25px 45px rgba(0, 0, 0, 0.1);
}
.login-pic {
  width: 130px;
  height: 130px;
  border-radius: 50%;
  border: 1px solid #efefef;
  position: absolute;
  left: 50%;
  transform: translate(-50%, -50%);
  background-color: #fff;
  img {
    width: 100%;
    height: 100%;
    border-radius: 50%;
    background-color: #eee;
  }
}
/deep/ .login-user .el-input__inner {
  border: 0;
  background: transparent;
}
.el-select {
  width: 100px;
}
.login-form {
  width: 100%;
  position: absolute;
  bottom: 0;
  padding: 0 20px;
  box-sizing: border-box;
}
.mybtn {
  display: flex;
  justify-content: center;
}
.drag {
  position: relative;
  background-color: #e8e8e8;
  width: 100%;
  height: 34px;
  line-height: 34px;
  text-align: center;
  margin-bottom: 20px;
  border-radius: 40px;
  overflow: hidden;
}
.handler {
  width: 40px;
  height: 32px;
  border: 1px solid #ccc;
  cursor: move;
}
.handler_bg {
  background: #fff
    url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAA3hpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNS1jMDIxIDc5LjE1NTc3MiwgMjAxNC8wMS8xMy0xOTo0NDowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDo0ZDhlNWY5My05NmI0LTRlNWQtOGFjYi03ZTY4OGYyMTU2ZTYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NTEyNTVEMURGMkVFMTFFNEI5NDBCMjQ2M0ExMDQ1OUYiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NTEyNTVEMUNGMkVFMTFFNEI5NDBCMjQ2M0ExMDQ1OUYiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTQgKE1hY2ludG9zaCkiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo2MTc5NzNmZS02OTQxLTQyOTYtYTIwNi02NDI2YTNkOWU5YmUiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6NGQ4ZTVmOTMtOTZiNC00ZTVkLThhY2ItN2U2ODhmMjE1NmU2Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+YiRG4AAAALFJREFUeNpi/P//PwMlgImBQkA9A+bOnfsIiBOxKcInh+yCaCDuByoswaIOpxwjciACFegBqZ1AvBSIS5OTk/8TkmNEjwWgQiUgtQuIjwAxUF3yX3xyGIEIFLwHpKyAWB+I1xGSwxULIGf9A7mQkBwTlhBXAFLHgPgqEAcTkmNCU6AL9d8WII4HOvk3ITkWJAXWUMlOoGQHmsE45ViQ2KuBuASoYC4Wf+OUYxz6mQkgwAAN9mIrUReCXgAAAABJRU5ErkJggg==')
    no-repeat center;
}
.handler_ok_bg {
  background: #fff
    url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAA3hpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNS1jMDIxIDc5LjE1NTc3MiwgMjAxNC8wMS8xMy0xOTo0NDowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDo0ZDhlNWY5My05NmI0LTRlNWQtOGFjYi03ZTY4OGYyMTU2ZTYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NDlBRDI3NjVGMkQ2MTFFNEI5NDBCMjQ2M0ExMDQ1OUYiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NDlBRDI3NjRGMkQ2MTFFNEI5NDBCMjQ2M0ExMDQ1OUYiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTQgKE1hY2ludG9zaCkiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDphNWEzMWNhMC1hYmViLTQxNWEtYTEwZS04Y2U5NzRlN2Q4YTEiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6NGQ4ZTVmOTMtOTZiNC00ZTVkLThhY2ItN2U2ODhmMjE1NmU2Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+k+sHwwAAASZJREFUeNpi/P//PwMyKD8uZw+kUoDYEYgloMIvgHg/EM/ptHx0EFk9I8wAoEZ+IDUPiIMY8IN1QJwENOgj3ACo5gNAbMBAHLgAxA4gQ5igAnNJ0MwAVTsX7IKyY7L2UNuJAf+AmAmJ78AEDTBiwGYg5gbifCSxFCZoaBMCy4A4GOjnH0D6DpK4IxNSVIHAfSDOAeLraJrjgJp/AwPbHMhejiQnwYRmUzNQ4VQgDQqXK0ia/0I17wJiPmQNTNBEAgMlQIWiQA2vgWw7QppBekGxsAjIiEUSBNnsBDWEAY9mEFgMMgBk00E0iZtA7AHEctDQ58MRuA6wlLgGFMoMpIG1QFeGwAIxGZo8GUhIysmwQGSAZgwHaEZhICIzOaBkJkqyM0CAAQDGx279Jf50AAAAAABJRU5ErkJggg==')
    no-repeat center;
}
.drag_bg {
  background-color: #7ac23c;
  height: 34px;
  width: 0px;
  border-radius: 40px 0px 0px 40px;
}
.drag_text {
  position: absolute;
  top: 0px;
  width: 100%;
  text-align: center;
  -moz-user-select: none;
  -webkit-user-select: none;
  user-select: none;
  -o-user-select: none;
  -ms-user-select: none;
}
.bottom {
  position: absolute;
  bottom: 20px;
  left: 50%;
  transform: translate(-50%);
}
</style>
