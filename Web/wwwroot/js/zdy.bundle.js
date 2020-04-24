"use strict";

var zdy = function () {

    var _ = {};

    _.user = {
        set: function (value) {
            return $.Deferred(function (defer) {
                Cookies.set("zdy_user", JSON.stringify(value), { expires: 1 });
                defer.resolve();
            }).promise();
        },
        get: function () {
            var user = Cookies.get("zdy_user");
            if (user !== undefined) {
                return JSON.parse(user);
            }
            return undefined;
        }
    };

    _.token = {
        set: function (value) {
            return $.Deferred(function (defer) {
                Cookies.set("zdy_token", value, { expires: 1 });
                defer.resolve();
            }).promise();
        },
        get: function () {
            return "Bearer " + Cookies.get("zdy_token");
        }
    };

    _.dictionary = {
        getItem: function (value, array, isNeedIndex) {
            var item = undefined;
            if (value !== null && value !== undefined && value !== "") {
                for (var i = 0; i < array.length; i++) {
                    if (array[i].Value.toLowerCase() === value.toLowerCase()) {
                        item = array[i];
                        break;
                    }
                }
            }
            if (item !== undefined && !!isNeedIndex) {
                item.Index = array.indexOf(item);
            }
            return item;
        },
        getText: function (value, array) {
            var item = _.dictionary.getItem(value, array);
            return item !== undefined ? item.Name : "";
        }
    };

    _.viewmodel = function () {
        var modelBase = function () {
            this.options = function (options) {
                this.model.call(this, options);
                //将属性填入viewModel
                if (options.fields && $.isArray(options.fields)) {
                    for (var i = 0; i < options.fields.length; i++) {
                        this[options.fields[i].field] = options.fields[i].value;
                    }
                    //原集合取消监控，做reset模版
                    this.fields = JSON.parse(ko.toJSON(options.fields));
                }
                return this;
            };
            this.reset = function () {
                for (var i = 0; i < this.fields.length; i++) {
                    this[this.fields[i].field](this.fields[i].value);
                }
            };
            this.getInstance = function () {
                return this;
            };
            this.import = function (options) {
                options.call(this);
                return this;
            };
            this.bind = function (modelName) {
                this.modelName = modelName;
                var views = modelName === undefined ? $('body') : $("[data-model='" + modelName + "']");
                if (views.length > 0) {
                    for (var i = 0; i < views.length; i++) {
                        ko.applyBindings(this, views[i]);
                        $(views[i]).fadeIn();
                    }
                }
                else {
                    throw new Error("Could not find 'data-model'!");
                }
                return this;
            };
            this.assignment = function (data) {
                data = JSON.parse(ko.toJSON(data));
                //查找表单字段
                for (var index in data.fields) {
                    var key = data.fields[index]["field"];
                    var value = data[key];
                    if ($.isArray(value)) {
                        //如果表单字段是个数组,就给他用逗号连接
                        data.fields[index]["value"] = value.join(',');
                    } else {
                        data.fields[index]["value"] = value;
                    }
                }
                return data;
            };
            this.valid = function () {
                return _.validate($("[data-model='" + this.modelName + "']"));
            };
            this.dictionary = undefined;
            this.dic = function () {
                if (typeof arguments[0] === "object") {
                    this.dictionary = arguments[0];
                } else if (typeof arguments[0] === "string") {
                    if (arguments.length < 2) return "";
                    var kind = arguments[0];
                    var value = arguments[1];
                    var isNeedIndex = !!arguments[2];
                    if (isNeedIndex) {
                        return _.dictionary.getItem(value + '', this.dictionary[kind], isNeedIndex);
                    } else {
                        return _.dictionary.getText(value + '', this.dictionary[kind]);
                    }
                } else {
                    return "";
                }
            };
            this.goto = function (data, e) {
                var url = $(e.target).data("url");
                if (url !== undefined) {
                    _.gotoPage(url);
                }
            };
        };

        var form = function () {
            var self = this;
            this.model = function (options) {

                this.fields = options.fields;

                this.dataAddUrl = options.dataAddUrl;

                this.add = function () {
                    if (!_.validate($("[data-model='" + self.modelName + "']"))) {
                        return $.Deferred(function (defer) {
                            defer.resolve(false);
                        }).promise();
                    }

                    if (self.beforeAdd && self.beforeAdd.call(arguments) === false) {
                        return $.Deferred(function (defer) {
                            defer.resolve(false);
                        }).promise();
                    }

                    var data = self.assignment(self);
                    return _.ajaxPost(this.dataAddUrl, data).done(function (rst) {
                        _.toastr.success("新增操作成功");
                        if (self.afterAdd) self.afterAdd(rst.Data, data);
                    });
                };

                this.dataUpdateUrl = options.dataUpdateUrl;

                this.update = function () {
                    if (!_.validate($("[data-model='" + self.modelName + "']"))) {
                        return $.Deferred(function (defer) {
                            defer.resolve(false);
                        }).promise();
                    }

                    if (self.beforeUpdate && self.beforeUpdate.call(arguments) === false) {
                        return $.Deferred(function (defer) {
                            defer.resolve(false);
                        }).promise();
                    }

                    var data = self.assignment(self);
                    return _.ajaxPost(self.dataUpdateUrl, data).done(function (rst) {
                        _.toastr.success("修改操作成功");
                        if (self.afterUpdate) self.afterUpdate(rst.Data, data);
                    });
                };

                this.dataDeleteUrl = options.dataDeleteUrl;

                this.delete = function () {
                    if (self.beforeDelete && self.beforeDelete.call(arguments) === false) {
                        return $.Deferred(function (defer) {
                            defer.resolve(false);
                        }).promise();
                    }

                    return _.confirm("确认要删除这条数据？").done(function () {
                        var data = self.assignment(self);
                        return _.ajaxPost(self.dataDeleteUrl, data).done(function (rst) {
                            _.toastr.success("删除操作成功");
                            if (self.afterDelete) self.afterDelete(rst.Data, data);
                        });
                    });
                };

            };
            this.load = function () {
                if (self.beforeLoad && self.beforeLoad.call(arguments) === false) {
                    return this;
                }

                function afterLoad(data) {
                    for (var item in data) {
                        if (self[item]) {
                            self[item](data[item]);
                        }
                    }
                    if (self.afterLoad) self.afterLoad(data);
                }

                if (typeof arguments[0] === 'object') {
                    var data = arguments[0];
                    afterLoad(data);
                } else if (typeof arguments[0] === 'string') {
                    var url = arguments[0];
                    var obj = arguments[1];
                    _.ajaxPost(url, obj).done(function (rst) {
                        var data = rst.Data;
                        afterLoad(data);
                    });
                }
                return this;
            };
            this.refresh = function () {
                self.reset();
            };
            _.viewmodel.modelBase.call(this);
        };

        var table = function () {
            var self = this;
            this.model = function (options) {
                //数据集
                this.recordSet = ko.observableArray();

                //排序
                //orderField，defaultOrderBy & isAsc: 当前排序字段名，默认排序字段名和方向（升序/降序）
                this.orderField = ko.observable(options.defaultOrderBy);
                this.isAsc = ko.observable(options.isAsc || false);

                //分页
                this.count = ko.observable();//总记录数
                this.totalPages = ko.observable();//总页数
                this.pageNumbers = ko.observableArray();//页码列表
                this.pageSize = ko.observable(options.pageSize || 20);//显示数
                this.pageSizeStart = ko.observable();//显示数开始值
                this.pageSizeEnd = ko.observable();//显示数结束值
                this.pageIndex = ko.observable(1);//当前页
                this.showStartPagerDot = ko.observable(false);//页面开始部分是否显示点号
                this.showEndPagerDot = ko.observable(false);//页面结束部分是否显示点号
                this.pagerCount = 8;//如果分页的页面太多，截取部分页面进行显示，默认设置显示9个页面

                //作为显示数据的表格的头部：显示文字和对应的字段名（辅助排序）
                this.headers = ko.observableArray(options.headers);

                //查询地址
                this.dataQueryUrl = options.dataQueryUrl;

                //查询条件：标签和输入值
                this.fields = options.fields;

                //Search按钮
                this.search = function () {
                    //点击Search按钮前执行
                    if (this.beforeSearch && this.beforeSearch.call(arguments) === false) {
                        return $.Deferred(function (defer) {
                            defer.resolve(false);
                        }).promise();
                    }

                    var data = this.assignment(self);
                    return _.ajaxPost(this.dataQueryUrl, {
                        PageIndex: data.pageIndex,
                        PageSize: data.pageSize,
                        OrderField: data.orderField,
                        IsAsc: data.isAsc,
                        Fields: data.fields
                    }).done(function (rst) {
                        self.recordSet(rst.Data.Item1);
                        self.count(rst.Data.Item2);
                        self.resetPageNumbders();
                        if (self.afterSearch) self.afterSearch(rst.Data, data);
                    });
                };

                //获取数据之后根据记录数重置页码
                this.resetPageNumbders = function () {
                    //计算显示页数
                    self.totalPages(Math.ceil(self.count() / self.pageSize()));
                    self.pageNumbers.removeAll();
                    var start = 1, end = self.pagerCount;
                    if (self.pageIndex() >= self.pagerCount) {
                        start = self.pageIndex() - Math.floor(self.pagerCount / 2);
                        self.showStartPagerDot(true);
                    } else {
                        self.showStartPagerDot(false);
                    }
                    end = start + self.pagerCount - 1;
                    if (end > self.totalPages()) {
                        end = self.totalPages();
                        self.showEndPagerDot(false);
                    } else {
                        self.showEndPagerDot(true);
                    }
                    for (var i = start; i <= end; i++) {
                        self.pageNumbers.push(i);
                    }
                    //计算显示条数
                    self.pageSizeStart((self.pageIndex() - 1) * self.pageSize() + 1);
                    self.pageSizeEnd(self.pageIndex() * self.pageSize() > self.count() ? self.count() : self.pageIndex() * self.pageSize());
                };

                //点击表格头部进行排序
                this.sort = function (header) {
                    if (self.orderField() === header.field) {
                        self.isAsc(!self.isAsc());
                    }
                    self.orderField(header.field);
                    self.pageIndex(1);
                    self.search();
                };

                //点击页码获取当前页数据
                this.turnPage = function (pageIndex) {
                    self.pageIndex(pageIndex);
                    self.search();
                };

                //首页
                this.firstPage = function () {
                    self.turnPage(1);
                };

                //末页
                this.lastPage = function () {
                    self.turnPage(self.totalPages());
                };

                //上一页
                this.prevPage = function () {
                    if (self.pageIndex() > 1) {
                        self.turnPage(self.pageIndex() - 1);
                    }
                };

                //下一页
                this.nextPage = function () {
                    if (self.pageIndex() < self.totalPages()) {
                        self.turnPage(self.pageIndex() + 1);
                    }
                };

                //选择值的字段
                this.selectField = options.selectField || "Id";

                //是否选择全部
                this.isSelectAll = ko.pureComputed(function () {
                    var array = self.recordSet().map(function (item) {
                        return item[self.selectField];
                    });

                    var array2 = self.selectData();

                    //求交集
                    var intersectArray = Array.intersect(array, array2);

                    //求差集
                    var minusArray = Array.minus(array, intersectArray);

                    return minusArray.length === 0;
                }, this);

                //选择全部方法
                this.selectAll = function () {
                    var array = self.recordSet().map(function (item) {
                        return item[self.selectField];
                    });

                    var array2 = self.selectData();

                    //上一次选择状态
                    var isSelectAll = self.isSelectAll();

                    var results = [];

                    if (isSelectAll) {
                        //不全选，求差集
                        results = Array.minus(array, array2);
                    } else {
                        //全选，求并集
                        results = Array.union(array, array2);
                    }
                    self.selectData(results);
                };

                //以选择的数据
                this.selectData = ko.observableArray();
            };
            this.load = function () {
                if (self.beforeLoad && self.beforeLoad.call(arguments) === false) {
                    return this;
                }
                this.search().done(function (rst) {
                    if (self.afterLoad) self.afterLoad(rst.Data);
                });
                return this;
            };
            this.refresh = function () {
                self.firstPage();
            };
            _.viewmodel.modelBase.call(this);
        };

        var editTable = function () {
            var self = this;
            this.model = function (options) {
                //数据行模型
                this.rowFields = options.rowFields || function () { };

                //点击AddRow按钮
                this.addRow = function () {
                    if (typeof self.rowFields === "function") {
                        var row = new self.rowFields();
                        row["new"] = true;
                        this.recordSet.unshift(row);
                    }
                    else {
                        throw new Error("'rowFields' must be constructor;like this  function () { this.fields1 = defaultvalue1 , this.fields2 = defaultvalue2 }");
                    }
                };

                //编辑行Add提交地址
                this.dataAddUrl = options.dataAddUrl;

                //点击编辑行Add按钮
                this.add = function (data, context) {
                    //表单验证
                    if (!_.validate($(context.toElement).closest("tr.editable-editrow"))) {
                        return false;
                    }

                    //点击Add按钮前执行
                    if (self.beforeAdd && self.beforeAdd.call(this, data, context) === false) {
                        return false;
                    }

                    var json = ko.toJSON(data);
                    _.ajaxPost(self.dataAddUrl, json).done(function (rst) {
                        self.edited(data, context);
                        self.search();
                        if (self.afterAdd) self.afterAdd(JSON.parse(json), rst);
                    });
                };

                //表单Update提交地址
                this.dataUpdateUrl = options.dataUpdateUrl;

                //点击Update按钮
                this.update = function (data, context) {
                    //表单验证
                    if (!_.validate($(context.toElement).closest("tr.editable-editrow"))) {
                        return false;
                    }

                    //点击Update按钮前执行
                    if (self.beforeUpdate && self.beforeUpdate.call(this, data, context) === false) {
                        return false;
                    }

                    var json = ko.toJSON(data);
                    _.ajaxPost(self.dataUpdateUrl, json).done(function (rst) {
                        self.edited(data, context);
                        self.search();
                        if (self.afterUpdate) self.afterUpdate(JSON.parse(json), rst);
                    });
                };

                // 表单Delete提交地址
                this.dataDeleteUrl = options.dataDeleteUrl;

                //点击Delete按钮
                this.delete = function (data) {
                    _.confirm("确认要删除这条数据？", function () {

                        //点击Delete按钮前执行
                        if (self.beforeDelete && self.beforeDelete.call(this, data, context) === false) {
                            return false;
                        }

                        if (data["new"]) {
                            self.recordSet.remove(data);
                            _.toastr.success("删除成功！");
                            if (self.afterDelete) self.afterDelete(JSON.parse(json), rst);
                        }
                        else {
                            var json = ko.toJSON(data);
                            _.ajaxPost(self.dataDeleteUrl, json).done(function (rst) {
                                self.recordSet.remove(data);
                                _.toastr.success("删除成功！");
                                if (self.afterDelete) self.afterDelete(JSON.parse(json), rst);
                            });
                        }

                    });
                };

                //列表提交地址
                this.allSubmitUrl = options.allSubmitUrl;

                //提交整个列表
                this.allSubmit = function () {
                    //表单验证
                    if (!_.validate($('[data-model=' + this.modelName + ']'))) {
                        return false;
                    }

                    //点击recordSetSubmit按钮前执行
                    if (self.beforeAllSubmit && self.beforeAllSubmit.call(this, data, context) === false) {
                        return false;
                    }

                    var json = ko.toJSON(self);
                    _.ajaxPost(self.allSubmitUrl, json).done(function (rst) {
                        if (self.allSubmited) self.allSubmited(JSON.parse(json), rst);
                    });
                };

                //显示edit行
                this.editing = function (data, context) {
                    if (context.target.nodeName.toLowerCase() === "a") {
                        var row = $(context.toElement).closest("tr.editable-datarow");
                        row.hide();
                        row.next().show();
                    }
                };

                //隐藏edit行
                this.edited = function (data, context) {
                    if (context.target.nodeName.toLowerCase() === "a") {
                        var row = $(context.toElement).closest("tr.editable-editrow");
                        row.hide();
                        row.prev().show();
                    }
                };

                _.viewmodel.table.model.call(this);
            };
            this.load = function () {
                if (self.beforeLoad && self.beforeLoad.call(arguments) === false) {
                    return $.Deferred(function (defer) {
                        defer.resolve(false);
                    }).promise();
                }
                return this.search().done(function () {
                    if (self.afterAdd) self.afterAdd(rst.Data);
                });
            };
            _.viewmodel.modelBase.call(this);
        };

        return {
            modelBase: modelBase,
            form: form,
            table: table,
            editTable: editTable
        };
    }();

    _.querymethod = {
        "equal": 0,//等于
        "lessThan": 1,//小于
        "greaterThan": 2,//大于
        "lessThanOrEqual": 3,//小于等于
        "greaterThanOrEqual": 4,//大于等于
        "like": 6,//Like
        "in": 7,//In
        "notEqual": 9,//大于
        "startsWith": 10,//Like
        "endsWith": 11,//Like
        "contains": 12,//Like
        "stdIn": 13//In
    };

    _.module = function () {
        return new _.viewmodel.modelBase();
    };

    _.form = function () {
        return new _.viewmodel.form();
    };

    _.table = function () {
        return new _.viewmodel.table();
    };

    _.editTable = function () {
        return new _.viewmodel.editTable();
    };

    _.cache = {};

    _.createCache = function (requestFunc) {
        return function (key, callback) {
            if (!_.cache[key]) {
                _.cache[key] = $.Deferred(function (defer) {
                    requestFunc(defer, key);
                }).promise();
            }
            return _.cache[key].done(callback);
        };
    };

    _.ajaxCache = _.createCache(function (defer, url) {
        $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function (request) {
                request.setRequestHeader("Token", _.token.get());
                request.setRequestHeader("SessionID", _.session.get());
            },
            success: defer.resolve,
            error: defer.reject
        });
    });

    _.aop = {
        before: function (target, method, advice) {
            var original = target[method];
            target[method] = function () {
                advice.apply(this, arguments);
                original.apply(target, arguments);
            };
            return target;
        },
        after: function (target, method, advice) {
            var original = target[method];
            target[method] = function () {
                original.apply(target, arguments);
                advice.apply(this, arguments);
                return target;
            };
            return target;
        },
        around: function (target, method, advice) {
            var original = target[method];
            target[method] = function () {
                advice.apply(this, arguments);
                original.apply(target, arguments);
                advice.apply(this, arguments);
                return target;
            };
            return target;
        }
    };

    _.validate = function () {
        var messages = {
            required: "必须填写该字段",
            remote: "请修正该字段",
            email: "请输入正确格式的电子邮件",
            url: "请输入合法的网址",
            date: "请输入合法的日期",
            dateISO: "请输入合法的日期 (ISO).",
            number: "请输入合法的数字",
            digits: "只能输入大于0的整数",
            ints: "只能输入整数",
            creditcard: "请输入合法的信用卡号",
            equalTo: "请再次输入相同的值",
            accept: "请输入拥有合法后缀名的字符串",
            year: "请输入正确的4位数年份",
            maxlength: "长度必须是小于等于 {0}",
            minlength: "长度必须是大于等于 {0}",
            rangelength: "长度必须介于 {0} 和 {1} 之间",
            range: "请输入一个介于 {0} 和 {1} 之间的值",
            max: "请输入一个最大为 {0} 的值",
            min: "请输入一个最小为 {0} 的值",
            ip: "请输入合法的IP地址",
            cn: "只能输入中文",
            en: "只能输入英文",
            loginName: "只能输入数字、26个英文字母或者下划线",
            mobile: '请输入正确的手机号码',
            phone: '请输入正确的电话号码'
        };

        var methods = {
            required: function (value) {
                if ($.isArray(value)) {
                    return value.length > 0;
                }
                return $.trim(value).length > 0;
            },
            email: function (value) {
                if (this.required(value)) {
                    return /^\S+@\S+\.\S{2,}$/.test(value);
                } else {
                    return "";
                }
            },
            url: function (value) {
                if (this.required(value)) {
                    return /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value);
                } else {
                    return "";
                }
            },
            date: function (value) {
                if (this.required(value)) {
                    return !/Invalid|NaN/.test(new Date(value).toString());
                } else {
                    return "";
                }
            },
            dateISO: function (value) {
                if (this.required(value)) {
                    return /^\d{4}[\/\-]\d{1,2}[\/\-]\d{1,2}$/.test(value);
                } else {
                    return "";
                }
            },
            number: function (value) {
                if (this.required(value)) {
                    return /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value);
                } else {
                    return "";
                }
            },
            digits: function (value) {
                if (this.required(value)) {
                    return /^\d+$/.test(value);
                } else {
                    return "";
                }
            },
            creditcard: function (value) {
                if (this.required(value)) {
                    if (/[^0-9 \-]+/.test(value)) {
                        return false;
                    }
                    var nCheck = 0,
                        nDigit = 0,
                        bEven = false;

                    value = value.replace(/\D/g, "");

                    for (var n = value.length - 1; n >= 0; n--) {
                        var cDigit = value.charAt(n);
                        nDigit = parseInt(cDigit, 10);
                        if (bEven) {
                            if ((nDigit *= 2) > 9) {
                                nDigit -= 9;
                            }
                        }
                        nCheck += nDigit;
                        bEven = !bEven;
                    }

                    return nCheck % 10 === 0;
                } else {
                    return "";
                }
            },
            minlength: function (value, param) {
                if (this.required(value)) {
                    var length = $.isArray(value) ? value.length : value.toString().trim().length;
                    return length >= param;
                } else {
                    return "";
                }
            },
            maxlength: function (value, param) {
                if (this.required(value)) {
                    var length = $.isArray(value) ? value.length : value.toString().trim().length;
                    return length <= param;
                } else {
                    return "";
                }
            },
            rangelength: function (value, param) {
                if (this.required(value)) {
                    var length = $.isArray(value) ? value.length : value.toString().trim().length;
                    return length >= param[0] && length <= param[1];
                } else {
                    return "";
                }
            },
            min: function (value, param) {
                if (this.required(value)) {
                    return value >= param;
                } else {
                    return "";
                }
            },
            max: function (value, param) {
                if (this.required(value)) {
                    return value <= param;
                } else {
                    return "";
                }
            },
            range: function (value, param) {
                if (this.required(value)) {
                    return value >= param[0] && value <= param[1];
                } else {
                    return "";
                }
            },
            equalTo: function (value, param) {
                if (this.required(value)) {
                    var target = $(param);
                    return value === target.val();
                } else {
                    return "";
                }
            },
            remote: function (value, param) {
                if (this.required(value)) {
                    var rst = false;
                    var data = {};
                    data["data"] = value;
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: param,
                        dataType: 'json',
                        data: data,
                        success: function (rst) {
                            rst = rst;
                        }
                    });
                    return rst;
                } else {
                    return "";
                }
            },
            year: function (value) {
                if (this.required(value)) {
                    if (!value.isPositiveInteger()) {
                        rtn = dataRule[i];
                    }
                    else {
                        if (thisVal.length !== 4) {
                            rtn = dataRule[i];
                        }
                    }
                }
                else {
                    return "";
                }
            },
            ip: function (value) {
                if (this.required(value)) {
                    if ("/^(\d+)\.(\d+)\.(\d+)\.(\d+)$/g".test(value)) {
                        if (RegExp.$1 < 256 && RegExp.$2 < 256 && RegExp.$3 < 256 && RegExp.$4 < 256) {
                            return true;
                        }
                    }
                    return false;
                } else {
                    return "";
                }
            },
            cn: function (value) {
                if (this.required(value)) {
                    return /^[\u4e00-\u9fa5]*$/.test(value);
                } else {
                    return "";
                }
            },
            en: function (value) {
                if (this.required(value)) {
                    return /^[a-zA-Z]*$/.test(value);
                } else {
                    return "";
                }
            },
            loginName: function (value) {
                if (this.required(value)) {
                    return /^\w*$/.test(value);
                } else {
                    return "";
                }
            },
            mobile: function (value) {
                return value.length === 11 && /^1[3|4|5|7|8]\d{9}$/.test(value);
            },
            phone: function (value) {
                return /^0\d{2,3}-?\d{7,8}$/.test(value) || /^\d{7,8}$/.test(value);
            }
        };
        var getValue = function (element) {
            if (element.is("span") || element.is("label")) {
                return element.html().trim();
            }
            var type = element.attr("type"),
                val = element.val();
            if (type === "radio" || type === "checkbox") {
                var array = new Array();
                $("input[name='" + element.attr("name") + "']:checked").each(function () {
                    array.push($(this).val());
                });
                return array;
            }

            if (typeof val === "string") {
                return val.replace(/\r/g, "");
            }
            return val;
        };
        var checkRules = function (element) {
            var dataRule = element.attr('data-rule');
            if (dataRule === undefined || dataRule === "") {
                return "";
            }
            var rules = dataRule.split(',');
            var val = getValue(element);
            var rst = "";
            for (var i = 0; i < rules.length; i++) {
                var rule;
                var param;
                if (rules[i].split(':').length > 1) {
                    rule = rules[i].split(':')[0];
                    if (rules[i].indexOf('[') !== -1 && rules[i].indexOf(']') !== -1) {
                        if (rules[i].indexOf('|') === -1) {
                            throw Error('多个参数必须用[]将数据扩起，并且用|分隔');
                        }
                        param = rules[i].toString().replace('[', '').replace(']', '').split('|');
                    } else {
                        param = rules[i].split(':')[1];
                    }
                } else {
                    rule = rules[i];
                }

                for (var j in methods) {
                    if (rule === j) {
                        if (methods[j](val, param) === false) {
                            rst = String.format(messages[j], param);
                        }
                    }
                }
            }
            return rst;
        };
        var highlight = function (element, errorMessage) {
            $(element).closest("div.form-group").addClass("has-error");
            var $error = $("<span class='help-block help-block-error'>" + errorMessage + "</span>");
            errorPlacement($error, element);
        };
        var errorPlacement = function (error, element) {
            if (element.closest(".form-group").find(".help-block-error").length === 0) {
                if (element.is(':checkbox')) {
                    error.insertAfter(element.closest('.md-checkbox-list, .md-checkbox-inline, .checkbox-list, .checkbox-inline'));
                } else if (element.is(':radio')) {
                    error.insertAfter(element.closest('.md-radio-list, .md-radio-inline, .radio-list,.radio-inline'));
                } else {
                    error.insertAfter(element);
                }
            }
        };
        var unhighlight = function () {
            $("div.form-group").each(function () {
                $(this).removeClass("has-error");
                $(this).find(".help-block-error").remove();
            });
        };

        var valid = function (container) {
            var self = this;

            if (container.length === 0) return false;

            unhighlight();

            var isValid = true;

            container.find('[data-rule]').each(function () {
                $(this).removeClass('error');
                var errorMessage = checkRules($(this));
                if (errorMessage !== "") {
                    isValid = false;
                    highlight($(this), errorMessage);
                }
            });

            if (!isValid) {
                _.toastr.error("待提交的数据中有一些错误，请检查！");
            }

            return isValid;
        };

        return valid;
    }();

    _.ajaxBaseUrl = "/api/v1/";

    _.ajaxGet = function (url) {
        return $.ajax({
            url: _.ajaxBaseUrl + url,
            dataType: "json",
            type: "GET",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", _.token.get());
                _.showLoading();
            },
            complete: function () {
                _.hideLoading();
            },
            error: _.ajaxError
        });
    };

    _.ajaxPost = function (url, data) {
        return $.ajax({
            url: _.ajaxBaseUrl + url,
            dataType: "json",
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", _.token.get());
                _.showLoading();
            },
            complete: function () {
                _.hideLoading();
            },
            error: _.ajaxError
        });
    };

    _.ajaxPostSilent = function (url, data) {
        return $.ajax({
            url: _.ajaxBaseUrl + url,
            dataType: "json",
            type: "POST",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", _.token.get());
            },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data
        });
    };

    _.ajaxError = function (request) {
        /// <summary>交互错误统一处理方法</summary>
        var message = '{0} Code:{1}';
        var status = request.status;
        switch (status) {
            case 401:
                _.alert.error('您的登录凭证过期,请重新登录').then(function () {
                    if (typeof window.gotoIndex === "function") {
                        window.gotoIndex();
                    } else {
                        window.parent.gotoIndex();
                    }
                });
                break;
            case 403:
                _.toastr.error('未获得权限，请联系管理员');
                break;
            case 404:
                _.alert.error('请求未找到').then(function () {
                    window.location.reload();//刷新当前页面.
                });
                break;
            case 500:
                _.toastr.error('内部服务器错误');
                break;
            case 510:
                _.toastr.error(request.responseJSON.Message);
                break;
            default:
                _.toastr.error(String.format(message, '未知错误', status));
                break;
        }
    };

    _.upload = function (options) {
        options = Object.assign({
            files: undefined,
            type: undefined,
            businessId: undefined,
            input: undefined,
            e: undefined
        }, options);

        if (options.files === undefined) {
            options.files = $(options.input).prop('files');
        }
        if (options.files.length > 0) {
            var data = new FormData();
            for (var i = 0; i < options.files.length; i++) {
                data.append(options.files[i].name, options.files[i]);
            }
            data.append('type', options.type);
            data.append('businessId', options.businessId || 0);
            return $.ajax({
                url: _.ajaxBaseUrl + "upload",
                type: "POST",
                cache: false,
                data: data,
                processData: false,
                contentType: false,
                dataType: "json",
                beforeSend: function (request) {
                    request.setRequestHeader("Authorization", _.token.get());
                    _.showLoading();
                },
                complete: function () {
                    _.hideLoading();
                },
                error: _.ajaxError
            }).done(function (rst) {
                if (rst.IsSuccess === true) {
                    _.toastr.success('上传成功');
                } else {
                    _.toastr.error('上传失败');
                }
                if (options.input) {
                    $(options.input).val("");
                }
            });
        }
    };

    _.showLoading = function () {
        if (window.parent.showLoading) {
            window.parent.showLoading();
        }
    };

    _.hideLoading = function () {
        if (window.parent.hideLoading) {
            window.parent.hideLoading();
        }
    };

    _.alert = {
        fire: function (type, message, title) {
            var options = {
                title: title,
                text: message,
                type: type,

                buttonsStyling: false,

                confirmButtonText: "&nbsp;&nbsp;&nbsp;OK&nbsp;&nbsp;&nbsp;",
                confirmButtonClass: "btn btn-danger"
            };

            if (window.parent.swal) {
                return window.parent.swal.fire(options);
            } else {
                return swal.fire(options);
            }
        },
        error: function (message, title) {
            title = title !== undefined && title !== "" ? title : "错误";

            return _.alert.fire("error", message, title);
        },
        warning: function (message, title) {
            title = title !== undefined && title !== "" ? title : "警告";

            return _.alert.fire("warning", message, title);
        },
        success: function (message, title) {
            title = title !== undefined && title !== "" ? title : "成功";

            return _.alert.fire("success", message, title);
        },
        info: function (message, title) {
            title = title !== undefined && title !== "" ? title : "信息";

            return _.alert.fire("info", message, title);
        },
        question: function (message, title) {
            title = title !== undefined && title !== "" ? title : "问题";

            return _.alert.fire("question", message, title);
        }
    };

    _.toastr = {
        show: function (type, message, title, onclick) {
            var options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };

            if (typeof onclick === 'function' && onclick !== undefined) {
                options.onclick = onclick;
            } else {
                options.onclick = null;
            }

            if (window.parent.toastr) {
                window.parent.toastr.options = options;
                window.parent.toastr[type](message, title);
            } else {
                toastr.options = options;
                toastr[type](message, title);
            }
        },
        error: function (message, title, onclick) {
            _.toastr.show("error", message, title, onclick);
        },
        warning: function (message, title, onclick) {
            _.toastr.show("warning", message, title, onclick);
        },
        success: function (message, title, onclick) {
            _.toastr.show("success", message, title, onclick);
        },
        info: function (message, title, onclick) {
            _.toastr.show("info", message, title, onclick);
        }
    };

    _.confirm = function (messgae) {
        var options = {
            title: '确认提示',
            text: messgae,
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: '确认',
            cancelButtonText: '取消',
            reverseButtons: true
        };

        return $.Deferred(function (defer) {
            var c;

            if (window.parent.swal) {
                c = window.parent.swal.fire(options);
            } else {
                c = swal.fire(options);
            }

            c.then(function (result) {
                if (result.value) {
                    defer.resolve(true);
                } else if (result.dismiss === 'cancel') {
                    defer.resolve(false);
                }
            });
        }).promise();
    };

    _.showModal = function (id) {
        return $.Deferred(function (defer) {
            $('#' + id).modal({
                backdrop: 'static',
                keyboard: false
            }).on("shown.bs.modal", function () {
                defer.resolve();
            });
        }).promise();
    };

    _.hideModal = function (id) {
        return $.Deferred(function (defer) {
            $('#' + id).modal('hide').on("hidden.bs.modal", function () {
                defer.resolve();
            });
        }).promise();
    };

    _.openWindow = function (page) {
        if (typeof window.openWindow === "function") {
            window.openWindow(page);
        }
        else {
            window.parent.openWindow(page);
        }
    };

    _.gotoPage = function (url) {
        if (typeof window.gotoPage === "function") {
            window.gotoPage(url);
        }
        else {
            window.parent.gotoPage(url);
        }
    };

    _.closePage = function (url) {
        if (url === undefined) {
            url = window.location.pathname;
            if (window.location.search !== undefined && window.location.search !== "") {
                url += window.location.search;
            }
        }
        if (typeof window.gotoPage === "function") {
            window.closePage(url);
        }
        else {
            window.parent.closePage(url);
        }
    };

    _.replacePage = function (url) {
        _.closePage();
        _.gotoPage(url);
    };

    _.colors = [
        "blue          ",
        "blue-hoki     ",
        "blue-steel    ",
        "blue-madison  ",
        "blue-chambray ",
        "blue-ebonyclay",
        "green           ",
        "green-meadow    ",
        "green-seagreen  ",
        "green-turquoise ",
        "green-haze      ",
        "green-jungle    ",
        "red             ",
        "red-pink        ",
        "red-sunglo      ",
        "red-intense     ",
        "red-thunderbird ",
        "red-flamingo    ",
        "yellow            ",
        "yellow-gold       ",
        "yellow-casablanca ",
        "yellow-crusta     ",
        "yellow-lemon      ",
        "yellow-saffron    ",
        "purple          ",
        "purple-plum     ",
        "purple-medium   ",
        "purple-studio   ",
        "purple-wisteria ",
        "purple-seance   "
    ];

    _.tools = {
        bytesToSize: function (bytes) {
            if (bytes === 0) return '0 B';

            var k = 1024;

            sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

            i = Math.floor(Math.log(bytes) / Math.log(k));

            //return (bytes / Math.pow(k, i)) + ' ' + sizes[i];
            //toPrecision(3) 后面保留一位小数，如1.0GB 
            return (bytes / Math.pow(k, i)).toPrecision(3) + ' ' + sizes[i];
        },
        uuId: function (len, radix) {

            var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
            var uuid = [], i;
            radix = radix || chars.length;

            if (len) {
                // Compact form
                for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
            } else {
                // rfc4122, version 4 form
                var r;

                // rfc4122 requires these characters
                uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
                uuid[14] = '4';

                // Fill in random data.  At i===19 set the high bits of clock sequence as
                // per rfc4122, sec. 4.1.5
                for (i = 0; i < 36; i++) {
                    if (!uuid[i]) {
                        r = 0 | Math.random() * 16;
                        uuid[i] = chars[i === 19 ? r & 0x3 | 0x8 : r];
                    }
                }
            }

            return uuid.join('');
        },
        guid: function () {
            function Guid(g) {

                var arr = new Array(); //存放32位数值的数组
                if (typeof g === "string") { //如果构造函数的参数为字符串
                    InitByString(arr, g);
                }
                else {
                    InitByOther(arr);
                }
                //返回一个值，该值指示 Guid 的两个实例是否表示同一个值。
                this.Equals = function (o) {
                    if (o && o.IsGuid) {
                        return this.ToString() === o.ToString();
                    }
                    else {
                        return false;
                    }
                };

                //Guid对象的标记
                this.IsGuid = function () { };
                //返回 Guid 类的此实例值的 String 表示形式。
                this.ToString = function (format) {
                    if (typeof format === "string") {
                        if (format === "N" || format === "D" || format === "B" || format === "P") {
                            return ToStringWithFormat(arr, format);
                        }
                        else {
                            return ToStringWithFormat(arr, "D");
                        }
                    }
                    else {
                        return ToStringWithFormat(arr, "D");
                    }
                };

                //由字符串加载
                function InitByString(arr, g) {
                    g = g.replace(/\{|\(|\)|\}|-/g, "");
                    g = g.toLowerCase();
                    if (g.length !== 32 || g.search(/[^0-9,a-f]/i) !== -1) {
                        InitByOther(arr);
                    }
                    else {
                        for (var i = 0; i < g.length; i++) {
                            arr.push(g[i]);
                        }
                    }
                }
                //由其他类型加载
                function InitByOther(arr) {
                    var i = 32;
                    while (i--) {
                        arr.push("0");
                    }
                }

                /*
                根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
                N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 
                B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} 
                P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) 
                */

                function ToStringWithFormat(arr, format) {
                    var str;
                    switch (format) {
                        case "N":
                            return arr.toString().replace(/,/g, "");
                        case "D":
                            str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);
                            str = str.replace(/,/g, "");
                            return str;
                        case "B":
                            str = ToStringWithFormat(arr, "D");
                            str = "{" + str + "}";
                            return str;
                        case "P":
                            str = ToStringWithFormat(arr, "D");
                            str = "(" + str + ")";
                            return str;
                        default:
                            return new Guid();
                    }
                }
            }
            function NewGuid() {
                var g = "";
                var i = 32;
                while (i--) {
                    g += Math.floor(Math.random() * 16.0).toString(16);
                }
                return new Guid(g);
            }
            return NewGuid().ToString();
        },
        rmbToUpper: function (currencyDigits) {
            // Constants:
            var MAXIMUM_NUMBER = 99999999999.99;
            // Predefine the radix characters and currency symbols for output:
            var CN_ZERO = "零";
            var CN_ONE = "壹";
            var CN_TWO = "贰";
            var CN_THREE = "叁";
            var CN_FOUR = "肆";
            var CN_FIVE = "伍";
            var CN_SIX = "陆";
            var CN_SEVEN = "柒";
            var CN_EIGHT = "捌";
            var CN_NINE = "玖";
            var CN_TEN = "拾";
            var CN_HUNDRED = "佰";
            var CN_THOUSAND = "仟";
            var CN_TEN_THOUSAND = "万";
            var CN_HUNDRED_MILLION = "亿";
            var CN_SYMBOL = "";
            var CN_DOLLAR = "元";
            var CN_TEN_CENT = "角";
            var CN_CENT = "分";
            var CN_INTEGER = "整";

            // Variables:
            var integral; // Represent integral part of digit number.
            var decimal; // Represent decimal part of digit number.
            var outputCharacters; // The output result.
            var parts;
            var digits, radices, bigRadices, decimals;
            var zeroCount;
            var i, p, d;
            var quotient, modulus;

            // Validate input string:
            currencyDigits = currencyDigits.toString();
            if (currencyDigits === "") {
                alert("Empty input!");
                return "";
            }
            if (currencyDigits.match(/[^,.\d]/) !== null) {
                alert("Invalid characters in the input string!");
                return "";
            }
            if (currencyDigits.match(/^((\d{1,3}(,\d{3})*(.((\d{3},)*\d{1,3}))?)|(\d+(.\d+)?))$/) === null) {
                alert("Illegal format of digit number!");
                return "";
            }

            // Normalize the format of input digits:
            currencyDigits = currencyDigits.replace(/,/g, ""); // Remove comma delimiters.
            currencyDigits = currencyDigits.replace(/^0+/, ""); // Trim zeros at the beginning.
            // Assert the number is not greater than the maximum number.
            if (Number(currencyDigits) > MAXIMUM_NUMBER) {
                alert("Too large a number to convert!");
                return "";
            }

            // Process the coversion from currency digits to characters:
            // Separate integral and decimal parts before processing coversion:
            parts = currencyDigits.split(".");
            if (parts.length > 1) {
                integral = parts[0];
                decimal = parts[1];
                // Cut down redundant decimal digits that are after the second.
                decimal = decimal.substr(0, 2);
            }
            else {
                integral = parts[0];
                decimal = "";
            }
            // Prepare the characters corresponding to the digits:
            digits = new Array(CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE);
            radices = new Array("", CN_TEN, CN_HUNDRED, CN_THOUSAND);
            bigRadices = new Array("", CN_TEN_THOUSAND, CN_HUNDRED_MILLION);
            decimals = new Array(CN_TEN_CENT, CN_CENT);
            // Start processing:
            outputCharacters = "";
            // Process integral part if it is larger than 0:
            if (Number(integral) > 0) {
                zeroCount = 0;
                for (i = 0; i < integral.length; i++) {
                    p = integral.length - i - 1;
                    d = integral.substr(i, 1);
                    quotient = p / 4;
                    modulus = p % 4;
                    if (d === "0") {
                        zeroCount++;
                    }
                    else {
                        if (zeroCount > 0) {
                            outputCharacters += digits[0];
                        }
                        zeroCount = 0;
                        outputCharacters += digits[Number(d)] + radices[modulus];
                    }
                    if (modulus === 0 && zeroCount < 4) {
                        outputCharacters += bigRadices[quotient];
                    }
                }
                outputCharacters += CN_DOLLAR;
            }
            // Process decimal part if there is:
            if (decimal !== "") {
                for (i = 0; i < decimal.length; i++) {
                    d = decimal.substr(i, 1);
                    if (d !== "0") {
                        outputCharacters += digits[Number(d)] + decimals[i];
                    }
                }
            }
            // Confirm and return the final output string:
            if (outputCharacters === "") {
                outputCharacters = CN_ZERO + CN_DOLLAR;
            }
            if (decimal === "") {
                outputCharacters += CN_INTEGER;
            }
            outputCharacters = CN_SYMBOL + outputCharacters;
            return outputCharacters;
        },
        randomColorCssClass: function () {
            var index = Number.random(0, _colors.length - 1);
            return colors[index];
        }
    };

    _.states = [
        { "color": "#fff", "background-color": "#4C87B9" },
        { "color": "#fff", "background-color": "#3FABA4" },
        { "color": "#fff", "background-color": "#8877A9" },
        { "color": "#fff", "background-color": "#32c5d2" },
        { "color": "#fff", "background-color": "#D05454" },
        { "color": "#fff", "background-color": "#9A12B3" },
        { "color": "#fff", "background-color": "#4B77BE" },
        { "color": "#fff", "background-color": "#22313F" },
        { "color": "#fff", "background-color": "#C8D046" },
        { "color": "#fff", "background-color": "#F2784B" }
    ];

    return _;
}();

ko.bindingHandlers.img = {
    update: function (el, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value === "" || value === undefined) {
            value = "/assets/global/img/no-image.jpg";
        }
        $(el).attr({ "src": value });
    }
};

ko.bindingHandlers.dateFormat = {
    init: function (el, valueAccessor, allBindingsAccessor, viewModel) {
        var allBindings = allBindingsAccessor(),
            dateFormat = ko.utils.unwrapObservable(allBindings.dateFormat),
            text = ko.utils.unwrapObservable(allBindings.text);
        if (typeof dateFormat === "string") {
            var val;
            val = (new Date(text)).format(dateFormat);
            $(el).html(val);
        }
    }
};

ko.bindingHandlers.dateDiff = {
    update: function (el, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var str = "";
        if (value !== "" || value !== undefined) {
            str = (new Date(value)).diff2();
        }
        $(el).text(str);
    }
};

ko.bindingHandlers.select2 = {
    init: function (el, valueAccessor, allBindingsAccessor, viewModel) {
        var options = valueAccessor(),
            allBindings = allBindingsAccessor();
        setTimeout(function () {
            $(el).addClass("select2").select2(options);
            if (allBindings.select2.multiple === true) {
                $(el).on("select2:opening select2:closing", function (event) {
                    var data = $(this).select2('data');
                    var rst = "";
                    if (data.length > 0) {
                        rst = data.map(function (item) {
                            return item.id;
                        }).join(",");
                    }
                    if (allBindings.select2.value) {
                        allBindings.select2.value(rst);
                    }
                });
            }
        }, 0);
    },
    update: function (el, valueAccessor, allBindingsAccessor) {
        var obj = valueAccessor(),
            allBindings = allBindingsAccessor();
        if (allBindings.select2.multiple === true) {
            var val = allBindings.select2.value() || '';
            if (val.length > 0) {
                $(el).val(val.split(","));
            } else {
                $(el).val([]);
            }
        }
        $(el).trigger('change');
    }
};

ko.bindingHandlers.state = {
    update: function (el, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value === undefined) {
            $(el).text("无状态");
        } else {
            var index = value.Index;
            var name = value.Name;
            $(el).text(name).css(_.states[index]);
        }
        if ($(el).parent()[0].tagName.toLowerCase() === 'td') {
            $(el).parent().css({ "width": "1%", "text-align": "center" });
        }
    }
};

//string extend
$.extend(String.prototype, {
    // 在字符串末尾追加字符串
    append: function (str) {
        /// <summary>
        /// 在字符串末尾追加字符串
        /// </summary>
        /// <param name="str" type="string">
        /// 追加字的符串
        /// </param>
        return this.concat(str);
    },
    // 删除指定索引位置的字符，索引无效将不删除任何字符
    removeAt: function (index) {
        /// <summary>
        /// 删除指定索引位置的字符，索引无效将不删除任何字符
        /// </summary>
        /// <param name="index" type="int">
        /// 索引值
        /// </param>
        if (index < 0 || index >= this.length) {
            return this.valueOf();
        } else if (index === 0) {
            return this.substring(1, this.length);
        } else if (index === this.length - 1) {
            return this.substring(0, this.length - 1);
        } else {
            return this.substring(0, index) + this.substring(index + 1);
        }
    },
    // 删除指定索引间的字符串.sIndex和eIndex所在的字符不被删除
    removeAtScope: function (sIndex, eIndex) {
        /// <summary>
        /// 删除指定索引间的字符串.sIndex和eIndex所在的字符不被删除符
        /// </summary>
        /// <param name="sIndex" type="int">
        /// 起始索引值
        /// </param>
        /// <param name="eIndex" type="int">
        /// 结束索引值
        /// </param>
        if (sIndex === eIndex) {
            return this.deleteCharAt(sIndex);
        } else {
            if (sIndex > eIndex) {
                var tIndex = eIndex;
                eIndex = sIndex;
                sIndex = tIndex;
            }
            if (sIndex < 0) sIndex = 0;
            if (eIndex > this.length - 1) eIndex = this.length - 1;
            return this.substring(0, sIndex + 1) + this.substring(eIndex, this.length);
        }
    },
    // 比较两个字符串是否相等,其实也可以直接使用===进行比较
    equals: function (str) {
        /// <summary>
        /// 比较两个字符串是否相等,其实也可以直接使用===进行比较
        /// </summary>
        /// <param name="str" type="string">
        /// 字符串
        /// </param>
        if (this.length !== str.length) {
            return false;
        } else {
            for (var i = 0; i < this.length; i++) {
                if (this.charAt(i) !== str.charAt(i)) {
                    return false;
                }
            }
            return true;
        }
    },
    // 比较两个字符串是否相等，不区分大小写
    equalsIgnoreCase: function (str) {
        /// <summary>
        /// 比较两个字符串是否相等，不区分大小写
        /// </summary>
        /// <param name="str" type="string">
        /// 字符串
        /// </param>
        if (this.length !== str.length) {
            return false;
        } else {
            var tmp1 = this.toLowerCase();
            var tmp2 = str.toLowerCase();
            return tmp1.equals(tmp2);
        }
    },
    // 将指定的字符串插入到指定的位置后面,索引无效将直接追加到字符串的末尾
    insert: function (index, str) {
        /// <summary>
        /// 将指定的字符串插入到指定的位置后面,索引无效将直接追加到字符串的末尾
        /// </summary>
        /// <param name="index" type="int">
        /// 索引值
        /// </param>
        /// <param name="str" type="string">
        /// 字符串
        /// </param>
        if (index < 0 || index >= this.length - 1) {
            return this.append(str);
        }
        return this.substring(0, index + 1) + str + this.substring(index + 1);
    },
    // 将指定的位置的字符设置为另外指定的字符或字符串.索引无效将直接返回不做任何处理
    setAt: function (index, str) {
        /// <summary>
        /// 将指定的位置的字符设置为另外指定的字符或字符串.索引无效将直接返回不做任何处理
        /// </summary>
        /// <param name="index" type="int">
        /// 索引值
        /// </param>
        /// <param name="str" type="string">
        /// 字符串
        /// </param>
        if (index < 0 || index > this.length - 1) {
            return this.valueOf();
        }
        return this.substring(0, index) + str + this.substring(index + 1);
    },
    // 清除两边的空格  
    trim: function () {
        /// <summary>
        /// 清除两边的空格
        /// </summary>
        return this.replace(/(^\s*)|(\s*$)/g, '');
    },
    // 除去左边空格 
    trimLeft: function () {
        /// <summary>
        /// 除去左边空格
        /// </summary>
        return this.replace(/^s+/g, "");
    },
    // 除去右边空格 
    trimRight: function () {
        /// <summary>
        /// 除去右边空格
        /// </summary>
        return this.replace(/s+$/g, "");
    },
    // 逆序
    reverse: function () {
        return this.split("").reverse().join("");
    },
    // 合并多个空白为一个空白  
    resetBlank: function () {
        /// <summary>
        /// 合并多个空白为一个空白 
        /// </summary>
        var regEx = /\s+/g;
        return this.replace(regEx, ' ');
    },
    // 保留数字  
    getNumber: function () {
        /// <summary>
        /// 保留数字
        /// </summary>
        var regEx = /[^\d]/g;
        return this.replace(regEx, '');
    },
    // 保留中文  
    getCN: function () {
        /// <summary>
        /// 保留中文
        /// </summary>
        var regEx = /[^\u4e00-\u9fa5\uf900-\ufa2d]/g;
        return this.replace(regEx, '');
    },
    // 保留字母 
    getEN: function () {
        /// <summary>
        /// 保留字母
        /// </summary>
        return this.replace(/[^A-Za-z]/g, "");
    },
    // String转化为Int  
    toInt: function () {
        /// <summary>
        /// String转化为Int
        /// </summary>
        return isNaN(parseInt(this)) ? this.toString() : parseInt(this);
    },
    // String转化为Float
    toFloat: function () {
        /// <summary>
        /// String转化为Int
        /// </summary>
        return isNaN(parseFloat(this)) ? this.toString() : parseFloat(this);
    },
    // String转化为字符数组
    toCharArray: function () {
        /// <summary>
        /// String转化为字符数组
        /// </summary>
        return this.split("");
    },
    // String转化为Json对象
    toJson: function () {
        /// <summary>
        /// String转化为Json对象
        /// </summary>
        return JSON.parse(this);
    },
    // 得到字节长度  
    getRealLength: function () {
        /// <summary>
        /// 得到字节长度
        /// </summary>
        var regEx = /^[\u4e00-\u9fa5\uf900-\ufa2d]+$/;
        if (regEx.test(this)) {
            return this.length * 2;
        } else {
            var oMatches = this.match(/[\0x00-\xff]/g);
            var oLength = this.length * 2 - oMatches.length;
            return oLength;
        }
    },
    // 从左截取指定长度的字串 
    left: function (n) {
        /// <summary>
        /// 从左截取指定长度的字串
        /// </summary>
        /// <param name="n" type="number">
        /// 长度
        /// </param>
        return this.slice(0, n);
    },
    // 从右截取指定长度的字串 
    right: function (n) {
        /// <summary>
        /// 从右截取指定长度的字串 
        /// </summary>
        /// <param name="n" type="number">
        /// 长度
        /// </param>
        return this.slice(this.length - n);
    },
    // HTML编码 
    htmlEncode: function () {
        /// <summary>
        /// HTML编码 
        /// </summary>
        var re = this;
        var q1 = [/x26/g, /x3C/g, /x3E/g, /x20/g];
        var q2 = ["&", "<", ">", " "];
        for (var i = 0; i < q1.length; i++)
            re = re.replace(q1[i], q2[i]);
        return re;
    },
    // Unicode转化 
    ascW: function () {
        /// <summary>
        /// Unicode转化
        /// </summary>
        var strText = "";
        for (var i = 0; i < this.length; i++)
            strText += "&#" + this.charCodeAt(i) + ";";
        return strText;
    },
    // 获取文件全名  
    getFileName: function () {
        /// <summary>
        /// 获取文件全名
        /// </summary>
        var regEx = /^.*\/([^\/\?]*).*$/;
        return this.replace(regEx, '$1');
    },
    // 获取文件扩展名  
    getExtensionName: function () {
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        var regEx = /^.*\/[^\/]*(\.[^\.\?]*).*$/;
        return this.replace(regEx, '$1');
    },
    //替换所有
    replaceAll: function (oldstr, newstr) {
        /// <summary>
        /// 替换所有
        /// </summary>
        /// <param name="oldstr" type="string">
        /// 旧字符
        /// </param>
        /// <param name="newstr" type="string">
        /// 新字符
        /// </param>
        return this.replace(new RegExp(oldstr, "gm"), newstr);
    },
    //是否正整数
    isPositiveInteger: function () {
        /// <summary>
        /// 是否正整数
        /// </summary>
        return new RegExp(/^[1-9]\d*$/).test(this);
    },
    //是否整数
    isInteger: function () {
        /// <summary>
        /// 是否整数
        /// </summary>
        return new RegExp(/^\d+$/).test(this);
    },
    //是否数字
    isNumber: function (value, element) {
        /// <summary>
        /// 是否数字
        /// </summary>
        return new RegExp(/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/).test(this);
    },
    //是否以某个字符串开始
    startsWith: function (pattern) {
        /// <summary>
        /// 是否以某个字符串开始
        /// </summary>
        return this.indexOf(pattern) === 0;
    },
    //是否以某个字符串结尾
    endsWith: function (pattern) {
        /// <summary>
        /// 是否以某个字符串结尾
        /// </summary>
        var d = this.length - pattern.length;
        return d >= 0 && this.lastIndexOf(pattern) === d;
    },
    //是否密码格式
    isValidPwd: function () {
        /// <summary>
        /// 是否密码格式
        /// </summary>
        return new RegExp(/^([_]|[a-zA-Z0-9]){6,32}$/).test(this);
    },
    //是否email格式
    isValidMail: function () {
        /// <summary>
        /// 是否email格式
        /// </summary>
        return new RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).test(this.trim());
    },
    //是否手机号码格式
    isPhone: function () {
        /// <summary>
        /// 是否手机号码格式
        /// </summary>
        return new RegExp(/(^([0-9]{3,4}[-])?\d{3,8}(-\d{1,6})?$)|(^\([0-9]{3,4}\)\d{3,8}(\(\d{1,6}\))?$)|(^\d{3,8}$)/).test(this);
    },
    //是否是链接
    isUrl: function () {
        /// <summary>
        /// 是否是链接
        /// </summary>
        return new RegExp(/^[a-zA-z]+:\/\/([a-zA-Z0-9\-\.]+)([-\w .\/?%&=:]*)$/).test(this);
    },
    //是否是外部链接
    isExternalUrl: function () {
        /// <summary>
        /// 是否是外部链接
        /// </summary>
        return this.isUrl() && this.indexOf("://" + document.domain) === -1;
    },
    //获取url参数值
    getUrlParm: function (name) {
        /// <summary>
        /// 获取url参数值
        /// </summary>
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(this);
        if (results === null)
            return "";
        else
            return results[1];
    },
    //向Url上附加参数
    appendUrlParm: function (param) {
        var url = this;
        if (typeof param === "string") {
            url = url + "?" + param;
        } else {
            var array = [];
            for (var key in param) {
                var item = param[key];
                array.push('&' + key + "=" + item);
            }
            var args = "";
            if (array.length > 0) {
                args = array.join('').substr(1);
                url = url + "?" + args;
            }
        }
        return url.replace(' ', '');
    }
});

$.extend(String, {
    format: function () {
        if (arguments.length === 0) {
            return '';
        }

        if (arguments.length === 1) {
            return arguments[0];
        }

        var reg = /{(\d+)?}/g;
        var args = arguments;
        var result = arguments[0].replace(reg, function ($0, $1) {
            return args[parseInt($1) + 1];
        });
        return result;
    }
});

//number extend
$.extend(Number.prototype, {
    // 数字补零
    zeroPadding: function (oCount) {
        /// <summary>
        /// 数字补零
        /// </summary>
        /// <param name="oCount" type="int">
        /// 补零个数
        /// </param>
        var strText = this.toString();
        while (strText.length < oCount) {
            strText = '0' + strText;
        }
        return strText;
    }
});

$.extend(Number, {
    random: function (min, max) {
        var Range = max - min;
        var Rand = Math.random();
        return min + Math.round(Rand * Range);
    }
});

//array extend
$.extend(Array.prototype, {
    // 数字数组由大到小排序
    maxToMin: function () {
        /// <summary>
        /// 数字数组由大到小排序
        /// </summary>
        var oValue;
        for (var i = 0; i < this.length; i++) {
            for (var j = 0; j <= i; j++) {
                if (this[i] > this[j]) {
                    oValue = this[i];
                    this[i] = this[j];
                    this[j] = oValue;
                }
            }
        }
        return this;
    },
    // 数字数组由小到大排序 
    minToMax: function () {
        /// <summary>
        /// 数字数组由小到大排序
        /// </summary>
        var oValue;
        for (var i = 0; i < this.length; i++) {
            for (var j = 0; j <= i; j++) {
                if (this[i] < this[j]) {
                    oValue = this[i];
                    this[i] = this[j];
                    this[j] = oValue;
                }
            }
        }
        return this;
    },
    // 获得数字数组中最大项  
    getMax: function () {
        /// <summary>
        /// 获得数字数组中最大项 
        /// </summary>
        var oValue = 0;
        for (var i = 0; i < this.length; i++) {
            if (this[i] > oValue) {
                oValue = this[i];
            }
        }
        return oValue;
    },
    // 获得数字数组中最小项  
    getMin: function () {
        /// <summary>
        /// 获得数字数组中最小项
        /// </summary>
        var oValue = 0;
        for (var i = 0; i < this.length; i++) {
            if (this[i] < oValue) {
                oValue = this[i];
            }
        }
        return oValue;
    },
    // 将数据批量加入到数据
    pushRange: function (items) {
        /// <summary>
        /// 将数据批量加入到数据
        /// </summary>
        /// <param name="items" type="array">
        /// 数组子项集合
        /// </param>
        var length = items.length;

        if (length !== 0) {
            for (var index = 0; index < length; index++) {
                this.push(items[index]);
            }
        }
    },
    // 清空数组
    clear: function () {
        /// <summary>
        /// 清空数组
        /// </summary>
        if (this.length > 0) {
            this.splice(0, this.length);
        }
    },
    // 数组是否为空
    isEmpty: function () {
        /// <summary>
        /// 数组是否为空
        /// </summary>
        if (this.length === 0)
            return true;
        else
            return false;
    },
    // 复制一个同样的数组
    clone: function () {
        /// <summary>
        /// 复制一个同样的数组
        /// </summary>
        var clonedArray = [];
        var length = this.length;

        for (var index = 0; index < length; index++) {
            clonedArray[index] = this[index];
        }
        return clonedArray;
    },
    // 是否包含某一项
    contains: function (item) {
        /// <summary>
        /// 是否包含某一项
        /// </summary>
        /// <param name="item" type="object">
        /// 数组子项
        /// </param>
        var index = this.indexOf(item);
        return index >= 0;
    },
    // 找到数组中某一项索引
    indexOf: function (item) {
        /// <summary>
        /// 找到数组中某一项索引
        /// </summary>
        /// <param name="item" type="object">
        /// 数组子项
        /// </param>
        var length = this.length;

        if (length !== 0) {
            for (var index = 0; index < length; index++) {
                if (this[index] === item) {
                    return index;
                }
            }
        }

        return -1;
    },
    // 将数据项插入到指定索引
    insert: function (index, item) {
        /// <summary>
        /// 将数据项插入到指定索引
        /// </summary>
        /// <param name="index" type="int">
        /// 索引值
        /// </param>
        /// <param name="item" type="object">
        /// 数组子项
        /// </param>
        this.splice(index, 0, item);
    },
    // 获得将数组每一项末尾追加字符的数组
    joinstr: function (str) {
        /// <summary>
        /// 获得将数组每一项末尾追加字符的数组
        /// </summary>
        /// <param name="str" type="string">
        /// 追加的字符
        /// </param>
        var new_arr = new Array(this.length);
        for (var i = 0; i < this.length; i++) {
            new_arr[i] = this[i] + str;
        }
        return new_arr;
    },
    // 删除指定数据项
    remove: function (item) {
        /// <summary>
        /// 删除指定数据项
        /// </summary>
        /// <param name="item" type="object">
        /// 数组子项
        /// </param>
        var index = this.indexOf(item);

        if (index >= 0) {
            this.splice(index, 1);
        }
    },
    // 通过索引删除指定数据项
    removeAt: function (index) {
        /// <summary>
        /// 删除指定数据项
        /// </summary>
        /// <param name="index" type="int">
        /// 索引值
        /// </param>
        this.splice(index, 1);
    },
    // each是一个集合迭代函数，它接受一个函数作为参数和一组可选的参数
    // 这个迭代函数依次将集合的每一个元素和可选参数用函数进行计算，并将计算得的结果集返回
    each: function (fn) {
        /// <summary>
        /// each是一个集合迭代函数，它接受一个函数作为参数和一组可选的参数
        /// 这个迭代函数依次将集合的每一个元素和可选参数用函数进行计算，并将计算得的结果集返回
        /// 例：var a = [1,2,3,4].each(function(x){return x < 0 ? x : null});
        /// </summary>
        /// <param name="fn" type="function">
        /// 筛选方法
        /// </param>
        /// <param name="param" type="object">
        /// 零个或多个可选的用户自定义参数
        /// </param>
        fn = fn || Function.K;
        var a = [];
        var args = Array.prototype.slice.call(arguments, 1);
        for (var i = 0; i < this.length; i++) {
            var res = fn.apply(this, [this[i], i].concat(args));
            if (res !== null) a.push(res);
        }
        return a;
    },
    //得到一个数组不重复的元素集合
    uniquelize: function () {
        /// <summary>
        /// 得到一个数组不重复的元素集合
        /// </summary>
        var ra = new Array();
        for (var i = 0; i < this.length; i++) {
            if (!ra.contains(this[i])) {
                ra.push(this[i]);
            }
        }
        return ra;
    }
});

$.extend(Array, {
    //求两个集合的补集
    complement: function (a, b) {
        /// <summary>
        /// 求两个集合的补集
        /// </summary>
        /// <param name="a" type="array">
        /// 集合a
        /// </param>
        /// <param name="b" type="array">
        /// 集合b
        /// </param>
        return Array.minus(Array.union(a, b), Array.intersect(a, b));
    },
    //求两个集合的交集
    intersect: function (a, b) {
        /// <summary>
        /// 求两个集合的交集
        /// </summary>
        /// <param name="a" type="array">
        /// 集合a
        /// </param>
        /// <param name="b" type="array">
        /// 集合b
        /// </param>
        return a.uniquelize().each(function (o) { return b.contains(o) ? o : null; });
    },
    //求两个集合的差集
    minus: function (a, b) {
        /// <summary>
        /// 求两个集合的差集
        /// </summary>
        /// <param name="a" type="array">
        /// 集合a
        /// </param>
        /// <param name="b" type="array">
        /// 集合b
        /// </param>
        return a.uniquelize().each(function (o) { return b.contains(o) ? null : o; });
    },
    //求两个集合的并集
    union: function (a, b) {
        /// <summary>
        /// 求两个集合的并集
        /// </summary>
        /// <param name="a" type="array">
        /// 集合a
        /// </param>
        /// <param name="b" type="array">
        /// 集合b
        /// </param>
        return a.concat(b).uniquelize();
    }
});

//date extend
$.extend(Date.prototype, {
    // 扩展Date格式化
    format: function (format) {
        /// <summary>
        /// 扩展Date格式化
        /// </summary>
        /// <param name="format" type="string">
        /// format信息
        /// </param>
        var o = {
            "M+": this.getMonth() + 1, //月份           
            "d+": this.getDate(), //日           
            "h+": this.getHours() % 12 === 0 ? 12 : this.getHours() % 12, //小时           
            "H+": this.getHours(), //小时           
            "m+": this.getMinutes(), //分           
            "s+": this.getSeconds(), //秒           
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度           
            "S": this.getMilliseconds() //毫秒           
        };
        var week = {
            "0": "\u65e5",
            "1": "\u4e00",
            "2": "\u4e8c",
            "3": "\u4e09",
            "4": "\u56db",
            "5": "\u4e94",
            "6": "\u516d"
        };
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        if (/(E+)/.test(format)) {
            format = format.replace(RegExp.$1, (RegExp.$1.length > 1 ? RegExp.$1.length > 2 ? "\u661f\u671f" : "\u5468" : "") + week[this.getDay() + ""]);
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
    },
    //计算时差
    diff: function (interval, objDate) {
        //若参数不足或 objDate 不是日期类型則回传 undefined  
        if (arguments.length < 2 || objDate.constructor !== Date) { return undefined; }
        switch (interval) {
            //计算秒差                                                          
            case 's': return parseInt((objDate - this) / 1000);
            //计算分差  
            case 'n': return parseInt((objDate - this) / 60000);
            //计算時差  
            case 'h': return parseInt((objDate - this) / 3600000);
            //计算日差  
            case 'd': return parseInt((objDate - this) / 86400000);
            //计算周差  
            case 'w': return parseInt((objDate - this) / (86400000 * 7));
            //计算月差  
            case 'm': return objDate.getMonth() + 1 + (objDate.getFullYear() - this.getFullYear()) * 12 - (this.getMonth() + 1);
            //计算年差  
            case 'y': return objDate.getFullYear() - this.getFullYear();
            //输入有误  
            default: return undefined;
        }
    },
    //计算时差
    diff2: function () {
        var dateTimeStamp = this.getTime();
        var minute = 1000 * 60;
        var hour = minute * 60;
        var day = hour * 24;
        var halfamonth = day * 15;
        var month = day * 30;
        var now = new Date().getTime();
        var diffValue = now - dateTimeStamp;
        if (diffValue < 0) { return; }
        var monthC = diffValue / month;
        var weekC = diffValue / (7 * day);
        var dayC = diffValue / day;
        var hourC = diffValue / hour;
        var minC = diffValue / minute;
        if (monthC >= 1) {
            result = "" + parseInt(monthC) + "月前";
        }
        else if (weekC >= 1) {
            result = "" + parseInt(weekC) + "周前";
        }
        else if (dayC >= 1) {
            result = "" + parseInt(dayC) + "天前";
        }
        else if (hourC >= 1) {
            result = "" + parseInt(hourC) + "小时前";
        }
        else if (minC >= 1) {
            result = "" + parseInt(minC) + "分钟前";
        } else
            result = "刚刚";
        return result;
    },
    //增加时间
    add: function (interval, number) {
        var dtTmp = this;

        switch (interval) {
            //增加秒
            case 's': return new Date(Date.parse(dtTmp) + 1000 * number);
            //增加分
            case 'n': return new Date(Date.parse(dtTmp) + 60000 * number);
            //增加時 
            case 'h': return new Date(Date.parse(dtTmp) + 3600000 * number);
            //增加日
            case 'd': return new Date(Date.parse(dtTmp) + 86400000 * number);
            //增加周
            case 'w': return new Date(Date.parse(dtTmp) + 86400000 * 7 * number);
            //增加季
            case 'q': return new Date(dtTmp.getFullYear(), dtTmp.getMonth() + number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
            //增加月
            case 'm': return new Date(dtTmp.getFullYear(), dtTmp.getMonth() + number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
            //增加年
            case 'y': return new Date(dtTmp.getFullYear() + number, dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());

        }
    }
});