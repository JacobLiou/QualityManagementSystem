/*
 * @Author: 林伟群
 * @Date: 2022-05-17 17:13:32
 * @LastEditTime: 2022-06-14 09:29:15
 * @LastEditors: 林伟群
 * @Description: 请输入文件功能类型
 * @FilePath: \frontend\src\store\modules\record.js
 */

const record = {
    state: {
        checkRecord: {}, // 选中的问题记录
        editProblem: {}, // 问题编辑的属性
        checkUser: [], // 选中的人员数组
        personnelType: '', // 选择人的类型
        isCheck: false, // 是否选中
        isBackPath: false, // 路径原路返回
        backQueryParam: {}, // 页面跳转保留参数
    },
    mutations: {
        SET_CHECK_RECORD: (state, data) => {
            state.checkRecord = data
        },
        SET_CHECK_USER: (state, data) => {
            state.checkUser = data
        },
        SET_CHECK_TYPE: (state, data) => {
            state.personnelType = data
        },
        SET_CHECK_TRUE: (state, data) => {
            state.isCheck = data
        },
        SET_CHECK_PATH: (state, data) => {
            state.isBackPath = data
        },
        SET_EDIT_PROBLRM: (state, data) => {
            state.editProblem = data
        },
        SET_BACK_QP: (state, data) => {
            console.log(data, 'data');
            state.backQueryParam = data
        }
    },

}

export default record