<template>
  <a-modal
    title="新增机构"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="formLoading">
      <a-form :form="form">
        <a-form-item label="机构类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-decorator="['orgtype', { rules: [{ required: true, message: '请选择机构类型！' }] }]">
            <a-radio v-for="(item, index) in typeEnumDataDropDown" :key="index" :value="parseInt(item.code)">
              {{ item.value }}</a-radio
            >
          </a-radio-group>
        </a-form-item>

        <a-form-item label="机构名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入机构名称"
            v-decorator="['name', { rules: [{ required: true, message: '请输入机构名称！' }] }]"
          />
        </a-form-item>

        <a-form-item label="唯一编码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入唯一编码"
            v-decorator="['code', { rules: [{ required: true, message: '请输入唯一编码！' }] }]"
          />
        </a-form-item>

        <a-form-item label="上级机构" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-tree-select
            v-decorator="['pid', { rules: [{ required: true, message: '请选择上级机构！' }] }]"
            style="width: 100%"
            :dropdownStyle="{ maxHeight: '300px', overflow: 'auto' }"
            :treeData="orgTree"
            placeholder="请选择上级机构"
            treeDefaultExpandAll
          >
            <span slot="title" slot-scope="{ id }">{{ id }} </span>
          </a-tree-select>
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
          <a-textarea
            :rows="4"
            placeholder="请输入备注(不超过200字节)"
            v-decorator="['remark']"
            maxlength="200"
            @change="handleTextarea"
          ></a-textarea>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { sysOrgAdd, getOrgTree } from '@/api/modular/system/orgManage'
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
      typeEnumDataDropDown: [],
      orgTree: [],
      visible: false,
      confirmLoading: false,
      formLoading: true,
      form: this.$form.createForm(this),
    }
  },
  created() {
    this.sysDictTypeDropDown()
  },
  methods: {
    // 初始化方法
    add() {
      this.visible = true
      this.getOrgTree()
    },
    handleTextarea(value) {
      if (value.target.value.trim()?.length > 200) {
        this.$message.warning('最长字数为200')
      }
    },
    /**
     * 获取字典数据
     */
    sysDictTypeDropDown(text) {
      sysDictTypeDropDown({
        code: 'org_type',
      }).then((res) => {
        this.typeEnumDataDropDown = res.data
      })
    },
    /**
     * 获取机构树，并加载于表单中
     */
    getOrgTree() {
      getOrgTree().then((res) => {
        this.formLoading = false
        if (!res.success) {
          this.orgTree = []
          return
        }
        this.orgTree = [
          {
            id: '-1',
            parentId: '0',
            title: '顶级',
            value: '0',
            pid: '0',
            children: res.data,
          },
        ]
      })
    },

    handleSubmit() {
      const {
        form: { validateFields },
      } = this
      this.confirmLoading = true
      validateFields((errors, values) => {
        if (!errors) {
          sysOrgAdd(values)
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
