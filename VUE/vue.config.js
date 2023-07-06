module.exports = {
  devServer: {
    proxy: {
      // 配置跨域
      '/api': {
        target: 'http://127.0.0.1:9659/',
        changOrigin: true,
        ws: true,
        pathRewrite: {
          '^/api': ''
        }
      }
    }
  }
}
