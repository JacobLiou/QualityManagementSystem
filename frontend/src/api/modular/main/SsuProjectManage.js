import { axios } from '@/utils/request'

/**
 * 查询项目
 *
 * @author lilulu
 */
export function SsuProjectPage(parameter) {
  return axios({
    url: '/SsuProject/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 项目列表
 *
 * @author lilulu
 */
export function SsuProjectList(parameter) {
  return axios({
    url: '/SsuProject/select',
    method: 'get',
    params: parameter
  })
}

/**
 * @description: 根据项目ID获取成员列表
 * @return {*}
 */
export function SsuProjectusers(parameter) {
  return axios({
    url: '/SsuProject/getprojectuser',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加项目
 *
 * @author lilulu
 */
export function SsuProjectAdd(parameter) {
  return axios({
    url: '/SsuProject/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑项目
 *
 * @author lilulu
 */
export function SsuProjectEdit(parameter) {
  return axios({
    url: '/SsuProject/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除项目
 *
 * @author lilulu
 */
export function SsuProjectDelete(parameter) {
  return axios({
    url: '/SsuProject/delete',
    method: 'post',
    data: parameter
  })
}
