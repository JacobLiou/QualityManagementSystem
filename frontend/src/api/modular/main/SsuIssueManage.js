import { axios } from '@/utils/request'


// 获取问题列表
export function SsuIssueColumnDis(parameter) {
  return axios({
    url: "/issue/column/display",
    method: "get",
    params: parameter
  })
}

// 设置问题列表
export function SsuIssueColumnUpdate(parameter) {
  return axios({
    url: "/issue/column/update",
    method: "post",
    data: parameter
  })
}


/**
 * 查询问题详情
 *
 * @author licong
 */
export function IssueDetail(parameter) {
  return axios({
    url: '/issue/detail',
    method: 'get',
    params: parameter
  })
}

/**
 * 查询问题记录
 *
 * @author licong
 */
export function IssuePage(parameter) {
  return axios({
    url: '/issue/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加问题记录
 *
 * @author licong
 */
export function IssueAdd(parameter) {
  return axios({
    url: '/issue/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑问题记录
 *
 * @author licong
 */
export function IssueEdit(parameter) {
  return axios({
    url: '/issue/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 执行问题记录
 *
 * @author licong
 */
export function IssueExecute(parameter) {
  return axios({
    url: '/issue/execute',
    method: 'post',
    data: parameter
  })
}

/**
 * 验证问题
 *
 * @author licong
 */
export function IssueValidate(parameter) {
  return axios({
    url: '/issue/validate',
    method: 'post',
    data: parameter
  })
}

/**
 * 挂起问题
 *
 * @author licong
 */
export function IssueHangup(parameter) {
  return axios({
    url: '/issue/hangup',
    method: 'post',
    data: parameter
  })
}

/**
 * 重分派问题
 *
 * @author licong
 */
export function IssueRedispatch(parameter) {
  return axios({
    url: '/issue/redispatch',
    method: 'post',
    data: parameter
  })
}

/**
 * 分派问题
 *
 * @author licong
 */
export function IssueDispatch(parameter) {
  return axios({
    url: '/issue/dispatch',
    method: 'post',
    data: parameter
  })
}

/**
 * 数据导出
 *
 * @author licong
 */
export function IssueExport(parameter) {
  return axios({
    url: '/issue/export',
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
export function IssueTemplate(parameter) {
  return axios({
    url: '/issue/template',
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
export function IssueImport(parameter) {
  return axios({
    url: '/issue/import',
    method: 'post',
    data: parameter
  })
}

/**
 * 附件上传后通知问题表保存附件id
 * 20220511 逻辑改动：由原来的后端上传并保存Id信息改为前端上传、后端保存Id
 *
 * @author licong
 */
export function IssueAttachmentSaveId(parameter) {
  return axios({
    url: '/issue/attachment/saveId',
    method: 'post',
    data: parameter
  })
}

/**
 * 附件下载时获取该问题所属的附件编号等信息
 *
 * @author licong
 */
export function IssueAttachmentInfoList(parameter) {
  return axios({
    url: '/issue/attachment/infoList',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除问题记录
 *
 * @author licong
 */
export function IssueDelete(parameter) {
  return axios({
    url: '/issue/delete',
    method: 'post',
    data: parameter
  })
}

/**
 * 操作记录
 *
 * @author licong
 */
export function IssueOperationPage(parameter) {
  return axios({
    url: '/issue/operation/page',
    method: 'post',
    data: parameter
  })
}

export function Downloadfile(res) {
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
