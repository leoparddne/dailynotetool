注意安装环境 .NET 5


## 最佳实践
1. 当日工作开始时打开程序
2. 完成一项任务或中断需要处理其他事务时添加记录
3. 当日结束工作后点击复制
4. 记录填写错误后可撤销最后一条数据随后重新编写
```
复制的内容将包含当日工作总时长以及各项任务的时长
```


## 其他
如果第一条记录的开始时间不正确，可手动重置开始时间为需要的时间
!!!此时需要先添加一条记录，然后再重置时间，且重置时选择的时间不能大于添加记录的时间

### 7.24
1. 重置按钮逻辑调整
2. 撤销最后一条
3. 重写ui

## 更新日志

### 8.31
1. 添加调整最后一条记录结束时间的功能(解决多条记录忘记条件的问题)

### 7.24
1. 根据调整后的重置时间更新第一条数据的时长
2. 复制时增加总时长计算
3. 日志、配置文件单独存储

### 5.26  
1. 基础日志功能