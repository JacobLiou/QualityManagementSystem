<template>
  <a-modal
    title="新增角色"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="角色类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-decorator="['roleType', { rules: [{ required: true, message: '请选择角色类型！' }] }]">
            <a-radio v-for="(item, index) in typeEnumDataDropDown" :key="index" :value="parseInt(item.code)">
              {{ item.value }}</a-radio
            >
          </a-radio-group>
        </a-form-item>
        <a-form-item label="角色名" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入角色名"
            v-decorator="['name', { rules: [{ required: true, message: '请输入角色名！' }] }]"
          />
        </a-form-item>

        <a-form-item label="唯一编码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入唯一编码"
            v-decorator="['code', { rules: [{ required: true, message: '请输入唯一编码！' }] }]"
          />
        </a-form-item>

        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序">
          <a-input-number
            placeholder="请输入排序"
            style="width: 100%"
            v-decorator="['sort', { initialValue: 100 }]"
            :min="1"
            :max="1000"
          />
        </a-form-item>

        <a-form-item label="备注" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-textarea :rows="4" placeholder="请输入备注" v-decorator="['remark']"></a-textarea>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { sysRoleAdd } from '@/api/modular/system/roleManage'
import { sysDictTypeDropDown } from '@/api/modular/system/dictManage'
export default {
  data() {
    return {
      labelCol: {
        xs: {
          span: 24,
        },
        sm: {
          span: 5,
        },
      },
      wrapperCol: {
        xs: {
          span: 24,
        },
        sm: {
          span: 15,
        },
      },
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      typeEnumDataDropDown: [],
    }
  },
  created() {
    this.sysDictTypeDropDown()
  },
  methods: {
    // 初始化方法
    add(record) {
      this.visible = true
    },
    /**
     * 获取字典数据
     */
    sysDictTypeDropDown(text) {
      sysDictTypeDropDown({
        code: 'role_type',
      }).then((res) => {
        this.typeEnumDataDropDown = res.data
      })
    },
    handleSubmit() {
      const {
        form: { validateFields },
      } = this
      this.confirmLoading = true
      validateFields((errors, values) => {
        if (!errors) {
          sysRoleAdd(values)
            .then((res) => {
              if (res.success) {
                this.$message.success('新增成功')
                this.visible = false
                this.confirmLoading = false
                this.$emit('ok', values)
                this.form.resetFields()
              } else {
                this.$message.error('新增失败：' + res.message)
              }
            })
            .finally((res) => {
              this.confirmLoading = false
            })
        } else {
          this.confirmLoading = false
        }
      })
    },
    handleCancel() {
      this.form.resetFields()
      this.visible = false
    },
  },
}
</script>
