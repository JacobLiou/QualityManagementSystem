import { axios } from '@/utils/request'

/**
 * 查询问题扩展属性
 *
 * @author licong
 */
export function SsuIssueExtendAttributePage (parameter) {
  return axios({
    url: '/SsuIssueExtendAttribute/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 问题扩展属性列表
 *
 * @author licong
 */
export function SsuIssueExtendAttributeList (parameter) {
  return axios({
    url: '/SsuIssueExtendAttribute/list',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加问题扩展属性
 *
 * @author licong
 */
export function SsuIssueExtendAttributeAdd (parameter) {
  return axios({
    url: '/SsuIssueExtendAttribute/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 批量添加
 *
 * @author licong
 */
export function SsuIssueExtendAttributeBatchAdd (parameter) {
  return axios({
    url: '/SsuIssueExtendAttribute/batch-add-struct',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑问题扩展属性
 *
 * @author licong
 */
export function SsuIssueExtendAttributeEdit (parameter) {
  return axios({
    url: '/SsuIssueExtendAttribute/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除问题扩展属性
 *
 * @author licong
 */
export function SsuIssueExtendAttributeDelete (parameter) {
  return axios({
    url: '/SsuIssueExtendAttribute/delete',
    method: 'post',
    data: parameter
  })
}
