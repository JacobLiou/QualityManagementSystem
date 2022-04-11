import { axios } from '@/utils/request'

/**
 * 查询问题管理
 *
 * @author wanghongyu
 */
export function SsuIssuesPage (parameter) {
  return axios({
    url: '/SsuIssues/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 问题管理列表
 *
 * @author wanghongyu
 */
export function SsuIssuesList (parameter) {
  return axios({
    url: '/SsuIssues/list',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加问题管理
 *
 * @author wanghongyu
 */
export function SsuIssuesAdd (parameter) {
  return axios({
    url: '/SsuIssues/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑问题管理
 *
 * @author wanghongyu
 */
export function SsuIssuesEdit (parameter) {
  return axios({
    url: '/SsuIssues/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除问题管理
 *
 * @author wanghongyu
 */
export function SsuIssuesDelete (parameter) {
  return axios({
    url: '/SsuIssues/delete',
    method: 'post',
    data: parameter
  })
}
