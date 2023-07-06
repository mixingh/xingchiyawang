import axios from 'axios'

// 调用 axios.create() 函数，创建一个 axios 的实例对象，用 request 来接收
const request = axios.create({
  // 指定请求的根路径
  baseURL: 'http://127.0.0.1:9659/api/',
  timeout: 30000
})

export default request
