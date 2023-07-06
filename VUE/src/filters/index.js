/* 数字金额逢三加， 比如 123,464.23 */
export function numberToCurrency(value) {
  if (!value) return '0.00'
  // 将数值截取，保留两位小数
  value = value.toFixed(2)
  // 获取整数部分
  const intPart = Math.trunc(value)
  // 整数部分处理，增加,
  const intPartFormat = intPart.toString().replace(/(\d)(?=(?:\d{3})+$)/g, '$1,')
  // 预定义小数部分
  let floatPart = '.00'
  // 将数值截取为小数部分和整数部分
  const valueArray = value.toString().split('.')
  if (valueArray.length === 2) { // 有小数部分
    floatPart = valueArray[1].toString() // 取得小数部分
    return intPartFormat + '.' + floatPart
  }
  return intPartFormat + floatPart
}

export function initDate(value) {
  if (value === '-') return '-'
  var date = new Date(value)
  var Y = date.getFullYear() + '年'
  var M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '月'
  var D = date.getDate() + '日'
  // var h = date.getHours() + ':'
  // var m = date.getMinutes() + ':'
  // var s = date.getSeconds()
  // var df = Y + M + D + h + m + s
  var df = Y + M + D
  return df
}
