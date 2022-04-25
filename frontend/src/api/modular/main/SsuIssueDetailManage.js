import { axios } from '@/utils/request'

/**
 * 查询详细问题记录
 *
 * @author licong
 */
export function SsuIssueDetailPage (parameter) {
  return axios({
    url: '/SsuIssueDetail/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 详细问题记录列表
 *
 * @author licong
 */
export function SsuIssueDetailList (parameter) {
  return axios({
    url: '/SsuIssueDetail/list',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加详细问题记录
 *
 * @author licong
 */
export function SsuIssueDetailAdd (parameter) {
  return axios({
    url: '/SsuIssueDetail/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑详细问题记录
 *
 * @author licong
 */
export function SsuIssueDetailEdit (parameter) {
  return axios({
    url: '/SsuIssueDetail/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除详细问题记录
 *
 * @author licong
 */
export function SsuIssueDetailDelete (parameter) {
  return axios({
    url: '/SsuIssueDetail/delete',
    method: 'post',
    data: parameter
  })
}
