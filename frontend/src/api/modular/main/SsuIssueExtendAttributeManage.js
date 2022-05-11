import { axios } from '@/utils/request'

/**
 * 查询问题扩展属性
 *
 * @author licong
 */
export function IssueExtAttrPage (parameter) {
  return axios({
    url: '/issue/extAttr/Page',
    method: 'get',
    params: parameter
  })
}

/**
 * 问题扩展属性列表
 *
 * @author licong
 */
export function IssueExtAttrListStruct (parameter) {
  return axios({
    url: '/issue/extAttr/listStruct',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加问题扩展属性
 *
 * @author licong
 */
export function IssueExtAttrAddStruct (parameter) {
  return axios({
    url: '/issue/extAttr/addStruct',
    method: 'post',
    data: parameter
  })
}

/**
 * 批量添加
 *
 * @author licong
 */
export function IssueExtAttrBatchAddStruct (parameter) {
  return axios({
    url: '/issue/extAttr/batchAddStruct',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑问题扩展属性
 *
 * @author licong
 */
export function IssueExtAttrEditStruct (parameter) {
  return axios({
    url: '/issue/extAttr/editStruct',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除问题扩展属性
 *
 * @author licong
 */
export function IssueExtAttrDeleteStruct (parameter) {
  return axios({
    url: '/issue/extAttr/deleteStruct',
    method: 'post',
    data: parameter
  })
}

/**
 * 数据导入
 *
 * @author licong
 */
export function IssueExtAttrImport (parameter) {
  return axios({
    url: '/issue/extAttr/import',
    method: 'post',
    data: parameter
  })
}

/**
 * 问题模板下载
 *
 * @author licong
 */
export function IssueExtAttrTemplate (parameter) {
  return axios({
    url: '/issue/extAttr/template',
    method: 'get',
    data: parameter,
    responseType: 'blob'
  })
}
