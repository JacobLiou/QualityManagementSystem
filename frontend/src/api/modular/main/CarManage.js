import { axios } from '@/utils/request'

/**
 * 查询车辆信息
 *
 * @author fuqiang
 */
export function CarPage (parameter) {
  return axios({
    url: '/Car/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 车辆信息列表
 *
 * @author fuqiang
 */
export function CarList (parameter) {
  return axios({
    url: '/Car/list',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加车辆信息
 *
 * @author fuqiang
 */
export function CarAdd (parameter) {
  return axios({
    url: '/Car/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑车辆信息
 *
 * @author fuqiang
 */
export function CarEdit (parameter) {
  return axios({
    url: '/Car/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除车辆信息
 *
 * @author fuqiang
 */
export function CarDelete (parameter) {
  return axios({
    url: '/Car/delete',
    method: 'post',
    data: parameter
  })
}
