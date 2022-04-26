import { axios } from '@/utils/request'

/**
 * 查询问题详情
 *
 * @author licong
 */
export function SsuIssueDetail (parameter) {
  return axios({
    url: '/SsuIssue/detail',
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
    url: '/SsuIssue/page',
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
    url: '/SsuIssue/list',
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
    url: '/SsuIssue/add',
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
    url: '/SsuIssue/edit',
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
    url: '/SsuIssue/execute',
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
    url: '/SsuIssue/validate',
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
    url: '/SsuIssue/hangup',
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
    url: '/SsuIssue/redispatch',
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
    url: '/SsuIssue/export',
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
    url: '/SsuIssue/delete',
    method: 'post',
    data: parameter
  })
}
