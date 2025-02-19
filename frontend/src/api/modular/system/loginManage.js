/**
 * 系统应用
 *
 * @author yubaoshan
 * @date 2020/5/26 19:06
 */
import { axios } from '@/utils/request'

/**
 * 登录
 *
 * @author yubaoshan
 * @date 2020/5/26 19:06
 */
export function login(parameter) {
  return axios({
    url: '/login',
    method: 'post',
    data: parameter
  })
}

export function loginForEmail(parameter) {
  console.log(parameter);
  return axios({
    url: '/emailLogin',
    method: 'post',
    data: parameter
  })
}

/**
 * @description: 获取企业微信登录扫描的url
 * @return {*}
 */
export function qiWeChatLoginUrl() {
  return axios({
    url: '/system/qyWechat/qywechatloginurl',
    method: "get"
  })
}

/**
 * @description: 企业微信登录获取token
 * @return {*}
 */
export function qyWeLoginToken(parameter) {
  return axios({
    url: "/system/qyWechat/qywechatgetlogintoken",
    method: "get",
    params: parameter
  })
}


/**
 * @description: 微信登录
 * @return {*}
 */
export function qyWechat() {
  return axios(
    {
      url: "/system/qyWechat/login",
      method: "get"
    }
  )
}

/**
 * 登出
 *
 * @author yubaoshan
 * @date 2020/5/26 19:07
 */
export function logout(parameter) {
  return axios({
    url: '/logout',
    method: 'get',
    params: parameter
  })
}

/**
 * 获取登录用户信息
 *
 * @author yubaoshan
 * @date 2020/5/26 19:08
 */
export function getLoginUser(parameter) {
  return axios({
    url: '/getLoginUser',
    method: 'get',
    params: parameter
  })
}

/**
 * 获取租户开关
 *
 * @author yubaoshan
 * @date 2020/9/5 1:24
 */
export function getTenantOpen(parameter) {
  return axios({
    url: '/getTenantOpen',
    method: 'get',
    params: parameter
  })
}

/**
 * 获取短信验证码
 *
 * @author yubaoshan
 * @date 2020/5/26 19:29
 */
export function getSmsCaptcha(parameter) {
  return axios({
    url: '/getSmsCaptcha',
    method: 'get',
    params: parameter
  })
}

export function getEmailCaptcha(parameter) {
  return axios({
    url: '/system/login/getEmailCaptcha',
    method: 'get',
    params: parameter
  })
}
/**
 * @description: 发送手机验证码
 * @return {*}
 */
export function sendSmscode(parameter) {
  return axios({
    url: "/system/register/sendemailcode",
    method: "post",
    data: parameter
  })
}
export function sendEmailcode(parameter) {
  return axios({
    url: "/system/register/sendemailcode",
    method: "get",
    data: parameter
  })
}


// 用户注册
export function userRegister(parameter) {
  return axios({
    url: "/system/register/userRegister",
    method: "post",
    data: parameter
  })
}

/**
 * 获取验证码开关
 *
 * @author Jax
 * @date 2021/1/22 00:00
 */
export function getCaptchaOpen(parameter) {
  return axios({
    url: '/getCaptchaOpen',
    method: 'get',
    params: parameter
  })
}

/**
 * 获取验证图片 以及token
 *
 * @author Jax
 * @date 2021/1/22 00:00
 */
export function reqGet(data) {
  return axios({
    url: '/captcha/get',
    method: 'post',
    data
  })
}

/**
 * 滑动或者点选验证
 *
 * @author Jax
 * @date 2021/1/22 00:00
 */
export function reqCheck(data) {
  return axios({
    url: '/captcha/check',
    method: 'post',
    data
  })
}
