// 设置异步延迟间隔
export function delay(interval = 0) {
  return new Promise(resolve => {
    const timer = setTimeout(_ => {
      clearTimeout(timer)
      resolve()
    }, interval)
  })
}

// 把文件按照二进制进行读取
export function readFile(file) {
  return new Promise(resolve => {
    const reader = new FileReader()
    reader.readAsBinaryString(file)
    reader.onload = ev => {
      resolve(ev.target.result)
    }
  })
}

// 字段对应表
export const character = {
  contactName: {
    text: '姓名',
    type: 'string'
  },
  contactEmail: {
    text: '邮箱',
    type: 'string'
  },
  contactPhone: {
    text: '电话',
    type: 'string'
  },
  contactAddress: {
    text: '地址',
    type: 'string'
  }
}
