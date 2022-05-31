/*
 * @Author: 林伟群
 * @Date: 2022-05-30 14:53:41
 * @LastEditTime: 2022-05-30 16:02:37
 * @LastEditors: 林伟群
 * @Description: 版本表维护接口
 * @FilePath: \frontend\src\api\modular\system\versionManage.js
 */
import { axios } from "@/utils/request";

/**
 * @description: 获取版本类别接口
 * @return {*}
 */
export function getalltypelist() {
    return axios({
        url: "/system/version/getalltypelist",
        method: 'post',
    })
}

/**
 * @description: 新增版本号
 * @return {*}
 */
export function addversion(parameter) {
    return axios({
        url: "/system/version/addversion",
        method: 'post',
        data: parameter
    })
}