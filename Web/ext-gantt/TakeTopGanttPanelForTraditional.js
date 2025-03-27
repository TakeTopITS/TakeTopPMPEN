
Ext.define("MyApp.DemoGanttPanel", {
    extend: "Gnt.panel.Gantt",
    requires: [
        'Gnt.plugin.TaskContextMenu',
        'Gnt.column.StartDate',
        'Gnt.column.EndDate',
        'Gnt.column.Duration',
        'Gnt.column.PercentDone',
        'Gnt.column.ResourceAssignment',
        'Sch.plugin.TreeCellEditing',
        'Sch.plugin.Pan'
    ],
    rightLabelField: 'Responsible',
    highlightWeekends: true,
    showTodayLine: true,
    loadMask: true,
    enableProgressBarResize: true,


    initComponent: function () {

        // 初始化撤销和重做栈
        this.undoStack = [];  // 撤销栈
        this.redoStack = [];  // 重做栈

        Ext.apply(this, {
            enableBaseline: true,
            baselineVisible: false,
            lockedGridConfig: {
                width: 300,
                title: '計畫列表',
                collapsible: true
            },

            // Experimental
            schedulerConfig: {
                collapsible: true,
                title: '<table><tr><td>甘特圖</td><td><img src="Images/UpDnArrow.png" width="18" height="24" border="0" alt="排序" onclick="javascript:SortByDate();"></td></tr></table>'

            },

            leftLabelField: {
                dataIndex: 'Name',
                editor: { xtype: 'textfield' }
            },
            rightLabelField: {
                dataIndex: 'AssignmentResources'
            },



            // Define an HTML template for the tooltip
            tooltipTpl: new Ext.XTemplate(
                '<h4 class="tipHeader">{Name}</h4>',
                '<table class="taskTip">',
                '<tr><td>開始:</td> <td align="right">{[Ext.Date.format(values.StartDate, "y-m-d")]}</td></tr>',
                '<tr><td>結束:</td> <td align="right">{[Ext.Date.format(Ext.Date.add(values.EndDate, Ext.Date.MILLI, -1), "y-m-d")]}</td></tr>',
                '<tr><td>進度:</td><td align="right">{PercentDone}%</td></tr>',
                '<tr><td>負責人:</td><td align="right">{Leader}</td></tr>',
                '</table>'
            ).compile(),

            eventRenderer: function (task) {
                return {
                    style: 'background-color: #' + task.data.TaskColor
                };
            },

            // Define the static columns
            columns: [
                //column displaying task color
                {
                    header: "標記",
                    xtype: 'templatecolumn',
                    width: 50,
                    tdCls: 'sch-column-color',
                    //locked: true,
                    field: {
                        allowBlank: false
                    },
                    tpl: '<div class="color-column-inner" style="background-color:#{TaskColor}">&nbsp;</div>',
                    listeners: {
                        click: function (panel, el, a, b, event, record) {
                            event.stopEvent();
                            this.rec = record;
                            this.showColumnMenu(el, event, record);
                        }
                    },
                    showColumnMenu: function (el, event, rec) {
                        //if color menu is not present, create a new Ext.menu.Menu instance
                        if (!this.colorMenu) {
                            this.colorMenu = new Ext.menu.Menu({
                                cls: 'gnt-locked-colormenu',
                                plain: true,
                                items: [
                                    Ext.create('Ext.ColorPalette', {
                                        allowReselect: true,
                                        listeners: {
                                            select: function (cp, color) {
                                                this.tpl.apply({ TaskColor: color });
                                                this.rec.set('TaskColor', color);
                                                this.colorMenu.hide();
                                            },
                                            scope: this
                                        }
                                    })

                                ]

                            });
                        }

                        this.colorMenu.showAt(event.xy);
                    }
                },
                //wbs列有問題
                new Gnt.column.WBS(),
                {
                    xtype: 'treecolumn',
                    header: '計畫名稱',
                    sortable: true,
                    dataIndex: 'Name',
                    width: 250,
                    //locked: true,
                    field: {
                        allowBlank: false
                    },
                    renderer: function (v, meta, r) {
                        if (!r.data.leaf) meta.tdCls = 'sch-gantt-parent-cell';

                        return v;
                    }
                },

                new Gnt.column.StartDate({
                    //xtype: 'startdatecolumn',
                    header: '開始時間'
                }),

                new Gnt.column.EndDate({
                    //xtype: 'startdatecolumn',
                    header: '結束時間'

                }),
                {
                    xtype: 'durationcolumn',
                    header: '工期',
                },
                {
                    xtype: 'percentdonecolumn',
                    width: 50,
                    header: '完成率'
                },


                //前置計畫， 工期，  部門，負責人，資源
                {

                    width: 60,
                    header: '前置計畫',
                    xtype: 'predecessorcolumn'
                },

                {

                    width: 60,
                    header: '後繼計畫',
                    xtype: 'successorcolumn'
                },

                //{

                //    width: 50,
                //    header: '部門'

                //},
                {
                    width: 60,
                    header: '負責人',
                    dataIndex: 'Leader',
                    editor: {
                        xtype: 'combobox',
                        name: "cmb_Leader",
                        store: ankData = new Ext.data.JsonStore({
                            root: 'list',
                            proxy: new Ext.data.HttpProxy({
                                url: 'handler/ProjectRelatedUserController.aspx?action=read&pid=' + pid.toString().substr(0, pid.toString().length - 2)
                            }),
                            autoLoad: true,
                            reader: new Ext.data.JsonReader({
                                root: "t_relateduser"
                            }),

                            fields: ['usercode', 'username']
                        }),
                        displayField: "username",
                        valueField: "usercode",
                        editable: false,
                        emptyText: '',
                        value: 1,
                    },
                },

                {
                    width: 60,
                    header: '計劃工時',
                    dataIndex: 'Workhour',
                    editor: { xtype: 'textfield' }
                },

                {
                    width: 60,
                    header: '實際工時',
                    dataIndex: 'Actualhour',
                    editor: { xtype: 'textfield' }
                },

                {
                    width: 60,
                    header: '费用預算',
                    dataIndex: 'Budget',
                    editor: { xtype: 'textfield' }
                },

                {
                    width: 60,
                    header: '實際費用',
                    dataIndex: 'Expense',
                    editor: { xtype: 'textfield' }
                },

                {
                    width: 60,
                    header: '要求數量',
                    dataIndex: 'RequireNumber',
                    editor: { xtype: 'textfield' }
                },

                {
                    width: 60,
                    header: '已完成量',
                    dataIndex: 'FinishedNumber',
                    editor: { xtype: 'textfield' }
                },

                {
                    width: 60,
                    header: '單價',
                    dataIndex: 'Price',
                    editor: { xtype: 'textfield' }
                },

                {
                    width: 60,
                    header: '單位',
                    dataIndex: 'UnitName',
                    editor: {
                        xtype: 'combobox',
                        name: "cmb_UnitName",
                        store: ankData = new Ext.data.JsonStore({
                            root: 'list',
                            proxy: new Ext.data.HttpProxy({
                                url: 'handler/ProjectRelatedUnitController.aspx?action=read'
                            }),
                            autoLoad: true,
                            reader: new Ext.data.JsonReader({
                                root: "T_JNUnit"
                            }),

                            fields: ['UnitName', 'SortNumber']
                        }),
                        displayField: "UnitName",
                        valueField: "UnitName",
                        editable: false,
                        emptyText: '',
                        value: 1,
                    },
                },


                //{
                //    width: 60,
                //    header: '單位',
                //    dataIndex: 'UnitName',
                //    editor: { xtype: 'textfield' }
                //},

                {
                    header: '優先順序',
                    width: 60,
                    editor: {
                        xtype: 'combobox',
                        name: "cmb_Priority",
                        store: Ext.create("Ext.data.Store", {
                            fields: ["Id", "Name"],
                            data: [{
                                Id: TaskPriority.Low,
                                Name: 'Low'
                            },
                            { Id: TaskPriority.Normal, Name: 'Normal' },
                            { Id: TaskPriority.High, Name: 'High' }]
                        }),
                        displayField: "Name",
                        valueField: "Id",
                        editable: false,
                        emptyText: 'Normal',
                        value: 1,
                    },
                    dataIndex: 'Priority',
                    renderer: function (v, m, r) {
                        switch (v) {
                            case TaskPriority.Low:
                                return 'Low';

                            case TaskPriority.Normal:
                                return 'Normal';

                            case TaskPriority.High:
                                return 'High';
                        }
                    }
                },

                //{
                //    xtype: 'booleancolumn',
                //    width: 50,

                //    header: '人工',

                //    dataIndex: 'ManuallyScheduled',

                //    field: {
                //    xtype: 'combo',
                //    store: ['true', 'false']
                //}
                //},

                //{
                //    //xtype : 'text',
                //    width: 50,
                //    header: '自訂的列',
                //    renderer: function () {
                //    return '自訂的列';
                //}
                //}


            ],

            // Define the buttons that are available for user interaction
            tbar: this.createToolbar()

        });

        // 初始化当前批次操作
        this.currentBatch = []; // 当前批次的操作

        this.callParent(arguments);

        // 初始化任务存储监听器
        this.initTaskStoreListeners();

        // 页面加载完成后，初始化按钮计数为 0，并监听鼠标移动事件
        Ext.onReady(function () {

            // 监听鼠标移动事件
            Ext.getBody().on('mousemove', this.onMouseMove, this);

            console.log('Component initialized, setting undo button text to 0'); // 调试日志
            this.updateUndoButtonText(this.getUndoButton(), 0); // 初始化撤销按钮文本为 0
            this.updateRedoButtonText(this.getRedoButton(), 0); // 初始化重做按钮文本为 0

        }.bind(this));
    },

    initTaskStoreListeners: function () {
        var taskStore = this.getTaskStore();

        // 监听任务添加事件
        taskStore.on('add', function (store, records) {
            console.log('Task added:', records); // 调试日志
            this.onTaskAdd(store, records);
        }, this);

        // 监听任务更新事件
        taskStore.on('update', function (store, record, operation) {
            console.log('Task updated:', record, operation); // 调试日志
            this.onTaskUpdate(store, record, operation);
        }, this);

        // 监听任务删除事件
        taskStore.on('remove', function (store, record) {
            console.log('Task removed:', record); // 调试日志
            this.onTaskRemove(store, record);
        }, this);
    },

    // 鼠标移动事件处理
    onMouseMove: function () {
        console.log('Mouse moved, current batch:', this.currentBatch); // 调试日志

        // 如果当前批次有操作，则将其记录到撤销栈中
        if (this.currentBatch.length > 0) {
            console.log('Recording current batch to undo stack:', this.currentBatch); // 调试日志
            this.undoStack.push(this.currentBatch); // 将当前批次的操作记录到撤销栈
            this.currentBatch = []; // 清空当前批次

            // 更新按钮计数
            this.updateUndoButtonText(this.getUndoButton(), this.undoStack.length);
            this.updateRedoButtonText(this.getRedoButton(), this.redoStack.length); // 更新重做按钮计数
        }

        // 移除鼠标移动事件监听（确保只执行一次）
        Ext.getBody().un('mousemove', this.onMouseMove, this);

        // 重新绑定鼠标移动事件，确保下一次操作也能触发
        Ext.getBody().on('mousemove', this.onMouseMove, this);
    },

    // 任务添加时记录操作
    onTaskAdd: function (store, records) {
        records.forEach(function (record) {
            this.recordOperation({
                type: 'add',       // 操作类型
                taskId: record.getId(), // 任务ID
                taskData: record.getData() // 任务数据
            });
        }, this);

        // 更新按钮计数
        this.updateUndoButtonText(this.getUndoButton(), this.undoStack.length + (this.currentBatch.length > 0 ? 1 : 0));
        this.updateRedoButtonText(this.getRedoButton(), this.redoStack.length); // 更新重做按钮计数
    },

    // 任务更新时记录操作
    onTaskUpdate: function (store, record, operation) {
        var oldData = record.modified; // 获取修改前的数据
        var newData = record.getData(); // 获取修改后的数据

        this.recordOperation({
            type: 'update',       // 操作类型
            taskId: record.getId(), // 任务ID
            oldValue: oldData,    // 修改前的值
            newValue: newData     // 修改后的值
        });

        // 更新按钮计数
        this.updateUndoButtonText(this.getUndoButton(), this.undoStack.length + (this.currentBatch.length > 0 ? 1 : 0));
        this.updateRedoButtonText(this.getRedoButton(), this.redoStack.length); // 更新重做按钮计数
    },

    // 任务删除时记录操作
    onTaskRemove: function (store, record) {
        this.recordOperation({
            type: 'delete',       // 操作类型
            taskId: record.getId(), // 任务ID
            taskData: record.getData() // 任务数据
        });

        // 更新按钮计数
        this.updateUndoButtonText(this.getUndoButton(), this.undoStack.length + (this.currentBatch.length > 0 ? 1 : 0));
        this.updateRedoButtonText(this.getRedoButton(), this.redoStack.length); // 更新重做按钮计数
    },

    // 记录操作到当前批次
    recordOperation: function (operation) {
        console.log('Recording operation:', operation); // 调试日志
        this.currentBatch.push(operation); // 将操作记录到当前批次

        // 更新按钮计数
        console.log('Current undoStack length:', this.undoStack.length); // 调试日志
        this.updateUndoButtonText(this.getUndoButton(), this.undoStack.length + 1); // 当前批次未提交，计数加 1
        this.updateRedoButtonText(this.getRedoButton(), this.redoStack.length); // 更新重做按钮计数
    },

    // 更新撤销按钮文本
    updateUndoButtonText: function (button, remainingUndos) {
        if (button && button.el) {
            // 找到 <div id="divNumber"> 并更新其内容
            var divNumber = button.el.down('#divNumber');
            if (divNumber) {
                console.log('Updating divNumber with:', remainingUndos); // 调试日志
                // 确保 remainingUndos 不小于 0
                var displayCount = Math.max(remainingUndos, 0);
                divNumber.update(displayCount);
            } else {
                console.error('divNumber not found in button'); // 调试日志
            }
        } else {
            console.error('Button or button.el is undefined'); // 调试日志
        }
    },

    // 更新重做按钮文本
    updateRedoButtonText: function (button, remainingRedos) {
        if (button && button.el) {
            // 找到 <div id="divRedoNumber"> 并更新其内容
            var divRedoNumber = button.el.down('#divRedoNumber');
            if (divRedoNumber) {
                console.log('Updating divRedoNumber with:', remainingRedos); // 调试日志
                // 确保 remainingRedos 不小于 0
                var displayCount = Math.max(remainingRedos, 0);
                divRedoNumber.update(displayCount);
            } else {
                console.error('divRedoNumber not found in button'); // 调试日志
            }
        } else {
            console.error('Button or button.el is undefined'); // 调试日志
        }
    },

    // 获取撤销按钮
    getUndoButton: function () {
        // 假设撤销按钮的 itemId 是 'undoButton'
        return this.down('#undoButton');
    },

    // 获取重做按钮
    getRedoButton: function () {
        // 假设重做按钮的 itemId 是 'redoButton'
        return this.down('#redoButton');
    },

    onUndo: function () {
        console.log('Undo button clicked, undoStack:', this.undoStack); // 调试日志

        if (this.undoStack.length > 0) {
            // 暂停任务存储的监听器，避免撤销操作触发新的记录
            this.pauseTaskStoreListeners();

            // 获取栈顶的批次操作
            var batchToUndo = this.undoStack.pop();
            console.log('Undoing batch:', batchToUndo); // 调试日志

            // 执行撤销操作
            for (var i = batchToUndo.length - 1; i >= 0; i--) {
                var operation = batchToUndo[i];
                this.performUndo(operation);
            }

            // 将撤销的批次操作放入重做栈
            this.redoStack.push(batchToUndo);

            // 恢复任务存储的监听器
            this.resumeTaskStoreListeners();

            // 更新按钮计数
            this.updateUndoButtonText(this.getUndoButton(), this.undoStack.length);
            this.updateRedoButtonText(this.getRedoButton(), this.redoStack.length); // 更新重做按钮文本
        } else {
            console.log('No operations to undo'); // 调试日志
            Ext.Msg.alert('提示', '沒有可撤銷的操作！');
        }
    },

    onRedo: function () {
        console.log('Redo button clicked, redoStack:', this.redoStack); // 调试日志

        if (this.redoStack.length > 0) {
            // 暂停任务存储的监听器，避免重做操作触发新的记录
            this.pauseTaskStoreListeners();

            // 获取栈顶的批次操作
            var batchToRedo = this.redoStack.pop();
            console.log('Redoing batch:', batchToRedo); // 调试日志

            // 执行重做操作
            for (var i = 0; i < batchToRedo.length; i++) {
                var operation = batchToRedo[i];
                this.performRedo(operation);
            }

            // 将重做的批次操作放入撤销栈
            this.undoStack.push(batchToRedo);

            // 恢复任务存储的监听器
            this.resumeTaskStoreListeners();

            // 更新按钮计数
            this.updateUndoButtonText(this.getUndoButton(), this.undoStack.length);
            this.updateRedoButtonText(this.getRedoButton(), this.redoStack.length); // 更新重做按钮文本
        } else {
            console.log('No operations to redo'); // 调试日志
            Ext.Msg.alert('提示', '没有可重做的操作！');
        }
    },

    // 暂停任务存储的监听器
    pauseTaskStoreListeners: function () {
        var taskStore = this.getTaskStore();
        taskStore.suspendEvents(); // 暂停所有事件
        console.log('Task store listeners paused'); // 调试日志
    },

    // 恢复任务存储的监听器
    resumeTaskStoreListeners: function () {
        var taskStore = this.getTaskStore();
        taskStore.resumeEvents(); // 恢复所有事件
        console.log('Task store listeners resumed'); // 调试日志
    },

    // 执行撤销操作
    performUndo: function (operation) {
        console.log('Performing undo:', operation); // 调试日志
        var taskStore = this.getTaskStore();
        var task = taskStore.getById(operation.taskId);

        if (!task) {
            console.error('Task not found for operation:', operation); // 调试日志
            return;
        }

        // 暂停任务存储的监听器，确保撤销操作不会触发新的事件
        this.pauseTaskStoreListeners();

        switch (operation.type) {
            case 'add':
                taskStore.remove(task); // 撤销添加操作：删除任务
                break;
            case 'update':
                task.set(operation.oldValue); // 撤销更新操作：恢复旧值
                break;
            case 'delete':
                taskStore.add(task); // 撤销删除操作：重新添加任务
                break;
        }

        // 恢复任务存储的监听器
        this.resumeTaskStoreListeners();

        this.refreshViews(); // 刷新视图
    },

    // 执行重做操作
    performRedo: function (operation) {
        console.log('Performing redo:', operation); // 调试日志
        var taskStore = this.getTaskStore();
        var task = taskStore.getById(operation.taskId);

        if (!task) {
            console.error('Task not found for operation:', operation); // 调试日志
            return;
        }

        // 暂停任务存储的监听器，确保重做操作不会触发新的事件
        this.pauseTaskStoreListeners();

        switch (operation.type) {
            case 'add':
                taskStore.add(task); // 重做添加操作：重新添加任务
                break;
            case 'update':
                task.set(operation.newValue); // 重做更新操作：应用新值
                break;
            case 'delete':
                taskStore.remove(task); // 重做删除操作：删除任务
                break;
        }

        // 恢复任务存储的监听器
        this.resumeTaskStoreListeners();

        this.refreshViews(); // 刷新视图
    },


    createToolbar: function () {
        return [

            {
                xtype: 'buttongroup',
                title: '',
                columns: 1,
                items: [

                    {
                        //iconCls: '<img src="Images/LeftRightArrow.png" width="18" height="24" border="0" alt="隐藏右边栏">',
                        text: '<img src="Images/LeftRightArrow.png" width="18" height="24" border="0" alt="隐藏右边栏">',
                        scope: this,
                        handler: function () {
                            ChangeMenu(1);
                        }
                    }

                    //{
                    //    //iconCls: '<img src="Images/LeftRightArrow.png" width="18" height="24" border="0" alt="隐藏右边栏">',
                    //    text: '<img src="Images/UpDnArrow.png" width="18" height="24" border="0" alt="隐藏右边栏">',
                    //    scope: this,
                    //    handler: function () {
                    //        $(".x-column-header-trigger").eq(3).click();
                    //    }
                    //}


                ]
            },

            {
                xtype: 'buttongroup',
                title: '視圖',
                columns: 3,
                items: [


                    {
                        iconCls: 'icon-prev',
                        text: '後退',
                        scope: this,
                        handler: function () {
                            this.shiftPrevious();
                        }
                    },
                    {
                        iconCls: 'icon-next',
                        text: '前進',
                        scope: this,
                        handler: function () {
                            this.shiftNext();
                        }
                    },
                    {
                        text: '合適螢幕',
                        iconCls: 'zoomfit',
                        handler: function () {
                            this.zoomToFit();

                            ////后退一步
                            //this.shiftPrevious();
                        },
                        scope: this
                    },

                    //{
                    //    text: '全屏',
                    //    iconCls: 'icon-fullscreen',
                    //    disabled: !this._fullScreenFn,
                    //    handler: function () {
                    //        this.showFullScreen();
                    //    },
                    //    scope: this
                    //},

                    {
                        text: '折疊所有',
                        iconCls: 'icon-collapseall',
                        scope: this,
                        handler: function () {
                            this.collapseAll();
                        }
                    },
                    {
                        text: '展開所有',
                        iconCls: 'icon-expandall',
                        scope: this,
                        handler: function () {
                            this.expandAll();
                        }
                    },
                    {
                        text: '初始大小',
                        iconCls: 'zoomfit',
                        handler: function () {
                            var sp = this.taskStore.getTotalTimeSpan();
                            this.switchViewPreset('weekAndDayLetter', sp.start, sp.end);

                            ////后退一步
                            //this.shiftPrevious();
                        },
                        scope: this
                    }
                ]
            },
            {
                xtype: 'buttongroup',
                title: '縮放',
                columns: 2,
                items: [{
                    text: '6 周',
                    scope: this,
                    handler: function () {
                        var sp = this.taskStore.getTotalTimeSpan();
                        this.switchViewPreset('weekAndMonth', sp.start, sp.end);
                    }
                },
                {
                    text: '10 周',
                    scope: this,
                    handler: function () {
                        var sp = this.taskStore.getTotalTimeSpan();
                        this.switchViewPreset('weekAndDayLetter', sp.start, sp.end);
                    }
                },
                {
                    text: '1 年',
                    scope: this,
                    handler: function () {
                        var sp = this.taskStore.getTotalTimeSpan();
                        this.switchViewPreset('monthAndYear', sp.start, sp.end);
                    }
                },
                {
                    text: '5 年',
                    scope: this,
                    handler: function () {
                        var sp = this.taskStore.getTotalTimeSpan();

                        this.switchViewPreset('monthAndYear', sp.start, sp.end);
                    }
                }
                ]
            },
            //編輯功能
            {
                xtype: 'buttongroup',
                title: '編輯',
                columns: 5,
                items: [

                    {
                        text: '添加計畫',
                        iconCls: 'icon-add',
                        scope: this,
                        handler: function () {
                            var original = this.getSelectionModel().selected.items[0];
                            var model = this.getTaskStore().model;

                            var newTask = new model({
                                leaf: true
                            });

                            newTask.setPercentDone(0);
                            newTask.setName("新計畫...");
                            //newTask.setStartDate((original && original.getStartDate()) || null);
                            ///newTask.setEndDate((original && original.getEndDate()) || null);
                            // newTask.setDuration((original && original.getDuration()) || null);
                            //newTask.setDurationUnit((original && original.getDurationUnit()) || 'd');

                            newTask.set(newTask.startDateField, (original && original.getStartDate()) || null);
                            newTask.set(newTask.endDateField, (original && original.getEndDate()) || null);
                            newTask.set(newTask.durationField, (original && original.getDuration()) || null);
                            newTask.set(newTask.durationUnitField, (original && original.getDurationUnit()) || 'd');

                            if (original) {
                                original.addTaskBelow(newTask);
                            } else {
                                this.taskStore.getRootNode().appendChild(newTask);
                            }
                        }

                    },
                    //{
                    //    enableToggle: true,
                    //    id: 'demo-readonlybutton',
                    //    scope: this,
                    //    text: '唯讀模式',
                    //    pressed: false,
                    //    handler: function () {
                    //        this.setReadOnly(Ext.getCmp("demo-readonlybutton").pressed);
                    //    }
                    //},

                    {
                        text: '計畫降級',
                        //iconCls : 'indent',
                        scope: this,
                        handler: function () {
                            var sm = this.lockedGrid.getSelectionModel();
                            this.taskStore.indent(sm.getSelection());
                        }
                    },
                    {
                        text: '計畫升級',
                        //iconCls : 'outdent',
                        scope: this,
                        handler: function () {
                            var sm = this.lockedGrid.getSelectionModel();
                            this.taskStore.outdent(sm.getSelection());
                        }
                    },

                    {
                        //iconCls: 'action',
                        text: '標紅拖期計畫',
                        scope: this,

                        handler: function (btn) {

                            Ext.Ajax.request({

                                url: "Handler/UpdateAllTardyPlansToRedColor.aspx?pid=" + pid,
                                success: function (msg) {    //這是處理後執行的函數，msg是處理頁返回的資料

                                    alert("標紅所有拖期計畫完成，如若不成功，說明你沒有此操作許可權！");
                                    location.reload();
                                    //this.refreshViews();

                                },
                                failure: function (msg) {    //这是处理后执行的函数，msg是处理页返回的数据

                                    alert("失败，请重新执行一次！");

                                }

                            });

                        }

                    },

                    {
                        //iconCls: 'action',
                        text: '取消標紅',
                        scope: this,

                        handler: function (btn) {

                            Ext.Ajax.request({

                                url: "Handler/DeleteAllTardyPlansToRedColor.aspx?pid=" + pid,
                                success: function (msg) {    //這是處理後執行的函數，msg是處理頁返回的資料
                                    /* alert(msg);*/

                                    alert("取消標紅所有拖期計畫完成，如若不成功，說明你沒有此操作許可權！");
                                    location.reload();
                                    //this.refreshViews();

                                },
                                failure: function (msg) {    //这是处理后执行的函数，msg是处理页返回的数据

                                    alert("失败，请重新执行一次！");

                                }

                            });

                        }

                    },

                    //{
                    //    //iconCls: 'action',
                    //    text: '一鍵全部轉任務',
                    //    scope: this,

                    //    handler: function (btn) {

                    //        Ext.Ajax.request({

                    //            url: "Handler/ProjectPlanOneStepTransferTask.ashx?pid=" + pid,
                    //            //success: function (msg) {    //這是處理後執行的函數，msg是處理頁返回的資料
                    //            //    alert(msg);
                    //            //}

                    //        });

                    //        alert("全部一鍵轉任務完成，如果不成功，說明有計劃沒有指定負責人或你沒有此操作許可權！");

                    //        this.refreshViews();

                    //    }

                    //},


                    {
                        //iconCls: 'action',
                        text: '同步基準計畫時間',
                        iconCls: 'action',
                        scope: this,

                        handler: function (btn) {

                            Ext.Ajax.request({

                                url: "Handler/SyncProjectBaselinePlanTime.aspx?pid=" + pid,
                                success: function (msg) {    //這是處理後執行的函數，msg是處理頁返回的資料
                                    /*     alert(msg);*/

                                    alert("基準時間同步完成，如若不成功，說明你沒有此操作許可權！");

                                },
                                failure: function (msg) {    //这是处理后执行的函数，msg是处理页返回的数据

                                    alert("失败，请重新执行一次！");

                                }

                            });

                            location.reload();

                        }

                    },

                    {
                        //iconCls: 'action',
                        text: '對比基準',
                        enableToggle: true,
                        pressed: false,
                        scope: this,
                        handler: function () {
                            this.el.toggleCls('sch-ganttpanel-showbaseline');
                        }
                    },


                    // 撤销按钮定义
                    {
                        text: '撤銷更新<div id="divNumber">0</div>', // 使用 HTML
                   /*     iconCls: 'icon-undo', // 修正图标类名*/
                        scope: this,
                        xtype: 'button',
                        enableHtml: true, // 启用 HTML 支持
                        itemId: 'undoButton', // 添加 itemId 以便获取按钮引用
                        handler: function () {
                            // 调用撤销逻辑
                            this.onUndo();
                            this.onUndo();
                          /*  this.onUndo();*/
                        },
                        listeners: {
                            afterrender: function (button) {
                                // 初始化按钮文本为 0
                                console.log('Initializing undo button text to 0'); // 调试日志
                                this.updateUndoButtonText(button, 0);
                            },
                            scope: this
                        }
                    },

                    // 重做按钮定义
                    {
                        text: '重做更新<div id="divRedoNumber">0</div>', // 使用 HTML
                     /*   iconCls: 'icon-redo', // 修正图标类名*/
                        scope: this,
                        xtype: 'button',
                        enableHtml: true, // 启用 HTML 支持
                        itemId: 'redoButton', // 添加 itemId 以便获取按钮引用
                        handler: function () {
                            // 调用重做逻辑
                            this.onRedo();
                            this.onRedo();
                           /* this.onRedo();*/
                        },
                        listeners: {
                            afterrender: function (button) {
                                // 初始化按钮文本为 0
                                console.log('Initializing redo button text to 0'); // 调试日志
                                this.updateRedoButtonText(button, 0);
                            },
                            scope: this
                        }
                    },
                    {
                        text: '保存',
                        //iconCls: 'icon-save',
                        scope: this,
                        handler: function () {
                            this.taskStore.sync({
                                success: function () {

                                    Ext.MessageBox.alert("恭喜", "資料保存成功！");
                                },
                                failure: function () {

                                    Ext.MessageBox.alert("錯誤", "資料保存錯誤");
                                }
                            });

                            this.refreshViews();
                        }
                    }

                ]
            },



            {
                xtype: 'buttongroup',
                title: '標進度',
                columns: 3,
                //defaults: { scale: "large" },
                items: [{
                    text: '0%',
                    scope: this,
                    handler: function () {
                        this.applyPercentDone(0);
                    }
                },
                {
                    text: '25%',
                    scope: this,
                    handler: function () {
                        this.applyPercentDone(25);
                    }
                },
                {
                    text: '50%',
                    scope: this,
                    handler: function () {
                        this.applyPercentDone(50);
                    }
                },
                {
                    text: '75%',
                    scope: this,
                    handler: function () {
                        this.applyPercentDone(75);
                    }
                },
                {
                    text: '100%',
                    scope: this,
                    handler: function () {
                        this.applyPercentDone(100);
                    }
                },
                {
                    //iconCls: 'action',
                    text: '列印',
                    enableToggle: true,
                    pressed: false,
                    scope: this,
                    handler: function () {
                        App.Gantt.gantt.print();

                    }
                }



                    //{
                    //    text: '打印',
                    //    scope: this,
                    //    handler: function () {

                    //        $("#ext-comp-1034-body").printArea();

                    //    }
                    //}

                ]
            },

            {
                xtype: 'buttongroup',
                title: '可擴展功能',
                columns: 2,
                items: [


                    //{
                    //    iconCls: 'action',
                    //    text: '高亮顯示7天以上的計畫',
                    //    scope: this,
                    //    handler: function (btn) {
                    //        this.taskStore.getRootNode().cascadeBy(function (task) {
                    //            if (Sch.util.Date.getDurationInDays(task.get('StartDate'), task.get('EndDate')) > 7) {
                    //                var el = this.getSchedulingView().getElementFromEventRecord(task);
                    //                el && el.frame('lime');
                    //            }
                    //        }, this);
                    //    }
                    //},


                    {
                        iconCls: 'togglebutton',
                        text: '前置計畫影響',
                        scope: this,
                        enableToggle: true,
                        handler: function (btn) {
                            this.setCascadeChanges(btn.pressed);
                        }
                    },


                    {
                        text: '路徑高亮顯示',
                        iconCls: 'togglebutton',
                        scope: this,
                        enableToggle: true,
                        handler: function (btn) {
                            var v = this.getSchedulingView();
                            if (btn.pressed) {
                                v.highlightCriticalPaths(true);
                            } else {
                                v.unhighlightCriticalPaths(true);
                            }
                        }
                    },

                    {
                        iconCls: 'action',
                        text: '滾動到最後一個計畫',
                        scope: this,
                        handler: function (btn) {
                            var latestEndDate = new Date(0),
                                latest;
                            this.taskStore.getRootNode().cascadeBy(function (task) {
                                if (task.get('EndDate') >= latestEndDate) {
                                    latestEndDate = task.get('EndDate');
                                    latest = task;
                                }
                            });
                            this.getSchedulingView().scrollEventIntoView(latest, true);
                        }
                    },


                    {
                        iconCls: 'action',
                        text: '刷新計畫',
                        enableToggle: true,
                        pressed: false,
                        scope: this,
                        handler: function () {
                            this.refreshViews();
                        }
                    },


                    //{
                    //    iconCls: 'togglebutton',
                    //    text: '過濾: 進度小於30%的計畫',
                    //    scope: this,
                    //    enableToggle: true,
                    //    toggleGroup: 'filter',
                    //    handler: function (btn) {
                    //        if (btn.pressed) {
                    //            this.taskStore.filter(function (task) {
                    //                return task.get('PercentDone') < 30;
                    //            });
                    //        } else {
                    //            this.taskStore.clearFilter();
                    //        }
                    //    }
                    //},


                    //{
                    //    xtype: 'textfield',
                    //    emptyText: '按關鍵字查找',
                    //    scope: this,
                    //    width: 150,
                    //    enableKeyEvents: true,
                    //    listeners: {
                    //        keyup: {
                    //            fn: function (field, e) {
                    //                var value = field.getValue();

                    //                if (value) {
                    //                    this.taskStore.filter('Name', field.getValue(), true, false);
                    //                } else {
                    //                    this.taskStore.clearFilter();
                    //                }
                    //            },
                    //            scope: this
                    //        },
                    //        specialkey: {
                    //            fn: function (field, e) {
                    //                if (e.getKey() === e.ESC) {
                    //                    field.reset();
                    //                }
                    //                this.taskStore.clearFilter();
                    //            },
                    //            scope: this
                    //        }
                    //    }
                    //},
                ]
            },
            //{
            //    xtype: 'buttongroup',
            //    title: '面板',
            //    columns: 2,
            //    defaults: { scale: "large" },
            //    items: [
            //        {
            //            text: '計畫面板',
            //            scope: this,
            //            handler: function () {
            //                var st = this.getView().getSelectionModel().getSelection();
            //                if (st.length > 0) {

            //                    this.taskEditor.showTask(st[0]);

            //                } else {
            //                    Ext.Msg.alert('提示', '請選擇一個計畫');
            //                }
            //            }
            //        },
            //        {
            //            text: '日曆面板',
            //            scope: this,
            //            handler: function () {
            //                var conf = {
            //                    calendar: this.taskStore.calendar
            //                };

            //                var editorWindow = new Gnt.widget.calendar.CalendarWindow(conf);
            //                editorWindow.show();
            //            }
            //        },


            //        {
            //        	text: '導入Project',
            //        	scope: this,
            //        	handler: function () {
            //        	var g = this;
            //        	var window =  new MSProjectImportPanel({
            //        		listeners : {
            //        		dataavailable: function(form, data) {
            //        		//msg('Success', 'Data from .mpp file loaded ');

            //        		g.taskStore.setRootNode(data.tasks);
            //        		g.resourceStore.loadData(data.resources);
            //        		g.assignmentStore.loadData(data.assignments);
            //        		g.dependencyStore.loadData(data.dependencies);

            //        		var column,
            //        		xtype;

            //        		for (var i=0, l=data.columns.length; i<l; i++){

            //        			xtype = data.columns[i].xtype;
            //        			delete data.columns[i].xtype;

            //        			column = Ext.widget(xtype, data.columns[i]);

            //        			g.lockedGrid.headerCt.add(column);
            //        		}
            //        		g.lockedGrid.headerCt.remove(0);
            //        		g.lockedGrid.getView().refresh();

            //        		g.expandAll();

            //        		var span = g.taskStore.getTotalTimeSpan();
            //        		if (span.start && span.end) {
            //        			g.setTimeSpan(span.start, span.end);
            //        		}
            //        	}
            //        	}
            //        	});
            //        	window.show();
            //        }
            //        }

            //    ]
            //},

        ];
    },

    applyPercentDone: function (value) {
        this.getSelectionModel().selected.each(function (task) { task.setPercentDone(value); });
    },

    showFullScreen: function () {
        this.el.down('.x-panel-body').dom[this._fullScreenFn]();
    },

    // Experimental, not X-browser
    _fullScreenFn: (function () {
        var docElm = document.documentElement;

        if (docElm.requestFullscreen) {
            return "requestFullscreen";
        }
        else if (docElm.mozRequestFullScreen) {
            return "mozRequestFullScreen";
        }
        else if (docElm.webkitRequestFullScreen) {
            return "webkitRequestFullScreen";
        }
    })()
});

TaskPriority = {
    Low: 0,
    Normal: 1,
    High: 2
};

Ext.define('MSProjectImportPanel', {
    //extend : 'Ext.form.Panel',
    extend: 'Ext.window.Window',
    width: 300,
    frame: true,
    title: '導入Project文件',
    bodyPadding: '10 10 0',

    defaults: {
        anchor: '100%',
        allowBlank: false,
        msgTarget: 'side',
        labelWidth: 50
    },
    initComponent: function () {
        this.addEvents('dataavailable');
        var w = this;
        Ext.apply(this, {
            items: [new Ext.form.Panel({
                items: [{
                    xtype: 'filefield',
                    id: 'form-file',
                    emptyText: '上傳 .mpp 文件',
                    fieldLabel: '選擇檔',
                    name: 'file',
                    buttonText: '',
                    buttonConfig: {
                        iconCls: 'upload-icon'
                    }
                }],
                buttons: [{
                    text: '上傳',
                    handler: function () {
                        var panel = this.up('form');
                        var form = panel.getForm();
                        if (form.isValid()) {
                            form.submit({
                                url: APIVAR.url.upload + '?pid=' + pid,
                                waitMsg: '正在載入資料...',
                                failure: function (form, action) {
                                    msg('上傳出錯', '請確認上傳檔案格式是否正確. 錯誤位置: ' + action.result.msg);
                                },
                                success: function (form, action) {
                                    //w.fireEvent('dataavailable', panel, action.result.data);
                                    this.refreshViews();
                                }
                            });
                        }
                    }
                },
                {
                    text: '重置',
                    handler: function () {
                        this.up('form').getForm().reset();
                    }
                }]
            })]

        });

        this.callParent(arguments);
    }
});

var msg = function (title, msg) {
    Ext.Msg.show({
        title: title,
        msg: msg,
        minWidth: 200,
        modal: true,
        icon: Ext.Msg.INFO,
        buttons: Ext.Msg.OK
    });
};


