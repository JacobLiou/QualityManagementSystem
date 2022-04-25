import { axios } from '@/utils/request'

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
