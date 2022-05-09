import { axios } from '@/utils/request'

/**
 * 查询问题详情
 *
 * @author licong
 */
export function SsuIssueDetail (parameter) {
  return axios({
    url: '/SsuIssue/Detail',
    method: 'get',
    params: parameter
  })
}

/**
 * 查询问题记录
 *
 * @author licong
 */
export function SsuIssuePage (parameter) {
  return axios({
    url: '/SsuIssue/Page',
    method: 'get',
    params: parameter
  })
}

/**
 * 问题记录列表
 *
 * @author licong
 */
export function SsuIssueList (parameter) {
  return axios({
    url: '/SsuIssue/List',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加问题记录
 *
 * @author licong
 */
export function SsuIssueAdd (parameter) {
  return axios({
    url: '/SsuIssue/Add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑问题记录
 *
 * @author licong
 */
export function SsuIssueEdit (parameter) {
  return axios({
    url: '/SsuIssue/Edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 执行问题记录
 *
 * @author licong
 */
export function SsuIssueExecute (parameter) {
  return axios({
    url: '/SsuIssue/Execute',
    method: 'post',
    data: parameter
  })
}

/**
 * 验证问题
 *
 * @author licong
 */
export function SsuIssueValidate (parameter) {
  return axios({
    url: '/SsuIssue/Validate',
    method: 'post',
    data: parameter
  })
}

/**
 * 挂起问题
 *
 * @author licong
 */
export function SsuIssueHangup (parameter) {
  return axios({
    url: '/SsuIssue/Hangup',
    method: 'post',
    data: parameter
  })
}

/**
 * 重分派问题
 *
 * @author licong
 */
export function SsuIssueRedispatch (parameter) {
  return axios({
    url: '/SsuIssue/Redispatch',
    method: 'post',
    data: parameter
  })
}

/**
 * 分派问题
 *
 * @author licong
 */
export function SsuIssueDispatch (parameter) {
  return axios({
    url: '/SsuIssue/Dispatch',
    method: 'post',
    data: parameter
  })
}

/**
 * 数据导出
 *
 * @author licong
 */
export function SsuIssueExport (parameter) {
  return axios({
    url: '/SsuIssue/Export',
    method: 'post',
    data: parameter,
    responseType: 'blob'
  })
}

/**
 * 问题模板下载
 *
 * @author licong
 */
export function SsuIssueTemplate (parameter) {
  return axios({
    url: '/SsuIssue/Template',
    method: 'get',
    data: parameter,
    responseType: 'blob'
  })
}

/**
 * 数据导入
 *
 * @author licong
 */
export function SsuIssueImportData (parameter) {
  return axios({
    url: '/SsuIssue/Import',
    method: 'post',
    data: parameter
  })
}

/**
 * 附件上传
 *
 * @author licong
 */
export function SsuIssueUploadFile (parameter) {
  return axios({
    url: '/SsuIssue/UploadFile',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除问题记录
 *
 * @author licong
 */
export function SsuIssueDelete (parameter) {
  return axios({
    url: '/SsuIssue/Delete',
    method: 'post',
    data: parameter
  })
}

/**
 * 操作记录
 *
 * @author licong
 */
export function OperationPage (parameter) {
  return axios({
    url: '/SsuIssueOperation/Page',
    method: 'post',
    data: parameter
  })
}

export function Downloadfile (res) {
  var blob = new Blob([res.data], { type: 'application/octet-stream;charset=UTF-8' })
  var contentDisposition = res.headers['content-disposition']
  var patt = new RegExp('filename=([^;]+\\.[^\\.;]+);*')
  var result = patt.exec(contentDisposition)
  var filename = result[1]
  var downloadElement = document.createElement('a')
  var href = window.URL.createObjectURL(blob) // 创建下载的链接
  var reg = /^["](.*)["]$/g
  downloadElement.style.display = 'none'
  downloadElement.href = href
  downloadElement.download = decodeURI(filename.replace(reg, '$1')) // 下载后文件名
  document.body.appendChild(downloadElement)
  downloadElement.click() // 点击下载
  document.body.removeChild(downloadElement) // 下载完成移除元素
  window.URL.revokeObjectURL(href)
}
