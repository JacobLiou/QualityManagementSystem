import { axios } from '@/utils/request'

/**
 * 查询产品
 *
 * @author lilulu
 */
export function SsuProductPage (parameter) {
  return axios({
    url: '/SsuProduct/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 产品列表
 *
 * @author lilulu
 */
export function SsuProductList (parameter) {
  return axios({
    url: '/SsuProduct/list',
    method: 'get',
    params: parameter
  })
}

/**
 * 添加产品
 *
 * @author lilulu
 */
export function SsuProductAdd (parameter) {
  return axios({
    url: '/SsuProduct/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑产品
 *
 * @author lilulu
 */
export function SsuProductEdit (parameter) {
  return axios({
    url: '/SsuProduct/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除产品
 *
 * @author lilulu
 */
export function SsuProductDelete (parameter) {
  return axios({
    url: '/SsuProduct/delete',
    method: 'post',
    data: parameter
  })
}
