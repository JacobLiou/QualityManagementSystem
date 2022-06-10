import { axios } from '@/utils/request'

/**
 * 查询人员组
 *
 * @author lilulu
 */
export function SsuGroupPage(parameter) {
  return axios({
    url: '/SsuGroup/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 人员组列表
 *
 * @author lilulu
 */
export function SsuGroupList(parameter) {
  return axios({
    url: '/SsuGroup/select',
    method: 'get',
    params: parameter
  })
}
/**
 * @description: 根据人员组id获取人员列表
 * @return {*}
 */
export function SsuGroupusers(parameter) {
  return axios({
    url: '/SsuGroup/getgroupusers',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加人员组
 *
 * @author lilulu
 */
export function SsuGroupAdd(parameter) {
  return axios({
    url: '/SsuGroup/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑人员组
 *
 * @author lilulu
 */
export function SsuGroupEdit(parameter) {
  return axios({
    url: '/SsuGroup/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除人员组
 *
 * @author lilulu
 */
export function SsuGroupDelete(parameter) {
  return axios({
    url: '/SsuGroup/delete',
    method: 'post',
    data: parameter
  })
}
