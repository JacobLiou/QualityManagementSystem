/*
 * @Author: 林伟群
 * @Date: 2022-05-17 17:13:32
 * @LastEditTime: 2022-05-18 17:07:13
 * @LastEditors: 林伟群
 * @Description: 请输入文件功能类型
 * @FilePath: \frontend\src\store\modules\record.js
 */


const record = {
    state: {
        checkRecord: {}, // 选中的问题记录
        checkUser: [], // 选中的人员数组
    },
    mutations: {
        SET_CHECK_RECORD: (state, data) => {
            state.checkRecord = data
        },
        SET_CHECK_USER: (state, data) => {
            state.checkUser = data
        }
    },

}

export default record