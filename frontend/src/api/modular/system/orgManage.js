
import { axios } from '@/utils/request'

/**
 * 获取机构树
 *
 * @author yubaoshan
 * @date 2020/4/26 12:08
 */
export function getOrgTree(parameter) {
  return axios({
    url: '/sysOrg/tree',
    method: 'get',
    params: parameter
  })
}

export function getSsuEmpOrgTree() {
  return axios({
    url: "/SsuEmpOrg/tree",
    method: "get",
  })
}

/**
 * @description: 根据机构ID获取人员列表
 * @return {*}
 */
export function getOrgUserList(parameter) {
  return axios({
    url: '/SsuEmpOrg/getorguser',
    method: 'get',
    params: parameter
  })
}

/**
 * 获取机构列表
 *
 * @author yubaoshan
 * @date 2020/5/11 12:59
 */
export function getOrgList(parameter) {
  return axios({
    url: '/sysOrg/list',
    method: 'get',
    params: parameter
  })
}

/**
 * 获取机构列表
 *
 * @author yubaoshan
 * @date 2020/5/11 16:17
 */
export function getOrgPage(parameter) {
  return axios({
    url: '/sysOrg/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 新增机构
 *
 * @author yubaoshan
 * @date 2020/5/11 13:56
 */
export function sysOrgAdd(parameter) {
  return axios({
    url: '/sysOrg/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑机构
 *
 * @author yubaoshan
 * @date 2020/5/11 13:56
 */
export function sysOrgEdit(parameter) {
  return axios({
    url: '/sysOrg/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除机构
 *
 * @author yubaoshan
 * @date 2020/5/11 12:59
 */
export function sysOrgDelete(parameter) {
  return axios({
    url: '/sysOrg/delete',
    method: 'post',
    data: parameter
  })
}

/**
 * @description: 根据项目ID和模块ID获取当前指派人列表
 * @return {*}
 */
export function getresponsibilityuser(parameter) {
  return axios({
    url: "/SsuEmpOrg/getresponsibilityuser",
    method: 'get',
    params: parameter
  })
}
