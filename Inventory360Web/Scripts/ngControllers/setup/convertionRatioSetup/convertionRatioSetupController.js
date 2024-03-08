myApp
    .controller('convertionRatioSetupController', ['$scope', '$q', '$filter', '$ngConfirm', 'dataTableRows', 'productGroupCommonService', 'convertionRatioSetupService',
        'productSubGroupCommonService', 'productCategoryCommonService', 'productBrandCommonService', 'productModelCommonService',
        'productCommonService', 'applicationBasePath','userService',
        function (
            $scope, $q, $filter, $ngConfirm, dataTableRows, productGroupCommonService, convertionRatioSetupService,
            productSubGroupCommonService, productCategoryCommonService, productBrandCommonService, productModelCommonService,
            productCommonService, applicationBasePath, userService
        ) {
            $scope.saveMode = true;
            $scope.problemId = 0;
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;


            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllConvertionRatioData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllConvertionRatioData();
                }
            };
            //variable
            $scope.convertionRatioProductDetail = [];
            $scope.lblSave = "Save";
            //-----object---------
            $scope.commonSetupConvertionRatio = {};
            function convertionRatioDTO(defaltData) {
                this.convertionRatioId = defaltData.convertionRatioId || 0;
                this.ratioNo = defaltData.ratioNo || "";
                this.ratioTitle = defaltData.ratioTitle || null;
                this.ratioDate = $filter('date')(defaltData.ratioDate, "M/d/yyyy") || $filter('date')(new Date(), "M/d/yyyy");
                this.description = defaltData.description || null;
                this.approved = defaltData.approved || "N";
                this.approvedBy = defaltData.approvedBy || null;
                this.approvedDate = defaltData.approvedDate || null;
                this.cancelReason = defaltData.cancelReason || "";
                this.commonSetupConvertionRatioDetail = defaltData.commonSetupConvertionRatioDetail || [];
            }
            function convertionRatioDetailDTO(defaltData) {
                this.convertionRatioDetailId = 0;
                this.convertionRatioId = 0;
                this.productFor = defaltData.productFor || "";
                this.productId = 0;
                this.productDimensionId = null;
                this.unitTypeId = 0;
                this.unitTypeName = "";
                this.quantity = null;
                this.remarks = ""
                //extra
                this.productForDetail = defaltData.productForDetail || "";
                this.productName = "";
                this.dimensionName = "";
                this.productNameJsonData = null;
                this.addedProductLists = [];
            }
            function init() {
                //-----object---------
                $scope.commonSetupConvertionRatio = new convertionRatioDTO({ approved: "N" });

                var convertionRatioDetailMObj = new convertionRatioDetailDTO({ productFor: "M", productForDetail: "Main Product" });
                $scope.convertionRatioProductDetail.push(convertionRatioDetailMObj);
                var convertionRatioDetailCObj = new convertionRatioDetailDTO({ productFor: "C", productForDetail: "Component Product" });
                $scope.convertionRatioProductDetail.push(convertionRatioDetailCObj);
            }
            //detail section Product
            $scope.getProductName = function (entity) {
                var q = (entity.productName === undefined || entity.productName == "*") ? '' : entity.productName;
                if (q.length >= 2 || entity.productName == "*") {
                    //when Main product then Except Component product Vice versa
                    var existingProductList = {};
                    if (entity.productFor == "M") {
                        existingProductList = {
                            query: q,
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionRatioProductDetail[1].addedProductLists)
                        };
                    }
                    else if (entity.productFor == "C") {
                        existingProductList = {
                            query: q,
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionRatioProductDetail[0].addedProductLists)
                        };
                    }
                    productCommonService.FetchProductNameExceptExistingProduct(existingProductList, q).then(function (value) {
                        //productCommonService.FetchProductName(q).then(function (value) {
                        entity.productNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });

                } else {
                    entity.productId = 0;
                    entity.productNameJsonData = null;
                }
            };
            $scope.fillProductTextbox = function (obj, entity) {
                entity.productId = Number(obj.Value);
                entity.productName = obj.Item;
                GetProductWiseDimension(entity);
                GetProductWiseUnitType(entity);
                entity.productNameJsonData = null;
            };
            //get product dimension
            var GetProductWiseDimension = function (entity) {
                productCommonService.FetchProductWiseDimension(
                    '', entity.productId
                ).then(function (value) {
                    // remove already added product dimention
                    var tmpProductDimensionIds = [];
                    if (entity.productFor == "M") {
                        var mainProduct = angular.copy($scope.convertionRatioProductDetail[1].addedProductLists);
                        $.each(mainProduct, function () {
                            if (this.productId == entity.productId) {
                                tmpProductDimensionIds.push(this.productDimensionId);
                            }
                        });
                    }
                    else if (entity.productFor == "C") {
                        var componentProduct = angular.copy($scope.convertionRatioProductDetail[0].addedProductLists);
                        $.each(componentProduct, function () {
                            if (this.productId == entity.productId) {
                                tmpProductDimensionIds.push(this.productDimensionId);
                            }
                        });
                    }
                    if (tmpProductDimensionIds.length > 0) {
                      var  filterData =  value.data.filter(x => !tmpProductDimensionIds.includes(x.Value))
                      entity.productDimensionDropDownJsonData = filterData;
                      if (filterData.length == 0)
                      {
                          $scope.clickToReset(entity);
                      }
                    }
                    else {
                        entity.productDimensionDropDownJsonData = value.data;
                    }
                    if (entity.productDimensionDropDownJsonData .length > 0) {
                        entity.productDimensionId = entity.productDimensionDropDownJsonData[0].Value;
                        $scope.getDimensionName(entity);
                    } else {
                        entity.dimensionName = '';
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get dimension
            $scope.getDimensionName = function (entity) {
                $.each(entity.productDimensionDropDownJsonData, function () {
                    if (this.Value === entity.productDimensionId) {
                        entity.dimensionName = this.Item;
                    }
                });
            };
            //get product unit type
            var GetProductWiseUnitType = function (entity) {
                productCommonService.FetchProductWiseUnitType(
                    '', entity.productId
                ).then(function (value) {
                    entity.unitTypeDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            entity.unitTypeId = this.Value;
                            $scope.getUnitTypeName(entity);
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };
            //get unit type
            $scope.getUnitTypeName = function (entity) {
                $.each(entity.unitTypeDropDownJsonData, function () {
                    if (this.Value === entity.unitTypeId) {
                        entity.unitTypeName = this.Item;
                    }
                });
            };
            //set Current Product Entity
            $scope.setCurrentProductEntity = function (entity) {
                $scope.currentEntity = entity;
            };
            //-------------------------------Modal-----------------------------------
            // Load subgroup and category by selecte group
            $scope.getSubGroupAndCategoryForDropdown = function () {
                $scope.productSubGroupDropDownJsonData = null;
                $scope.productCategoryDropDownJsonData = null;
                $scope.modelDropDownJsonData = null;
                if (Number($scope.productGroupId) > 0) {
                    SequenceCall({ GetProductSubGroupForDropdown, GetProductCategoryForDropdown, GetProductModelForDropdown });
                }
            };

            $scope.getProductModelForDropdown = function () {
                GetProductModelForDropdown();
            };

            $scope.getModalProductName = function () {
                var q = $scope.productName === undefined ? '' : $scope.productName;

                if (q.length >= 2) {
                    //when Main product then Except Component product Vice versa
                    var existingProductList = {};
                    if ($scope.currentEntity.productFor == "M") {
                        existingProductList = {
                            query: q,
                            productGroupId: $scope.productGroupId,
                            productSubGroupId: ($scope.productSubGroupId == undefined ? '0' : $scope.productSubGroupId),
                            productCategoryId: ($scope.productCategoryId == undefined ? '0' : $scope.productCategoryId),
                            brandId: $scope.brandId,
                            model: (($scope.modelId == undefined || $scope.modelId == 0) ? '' : $scope.modelId),
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionRatioProductDetail[1].addedProductLists)
                        };
                    }
                    else if ($scope.currentEntity.productFor == "C") {
                        existingProductList = {
                            query: q,
                            productGroupId: $scope.productGroupId,
                            productSubGroupId: ($scope.productSubGroupId == undefined ? '0' : $scope.productSubGroupId),
                            productCategoryId: ($scope.productCategoryId == undefined ? '0' : $scope.productCategoryId),
                            brandId: $scope.brandId,
                            model: (($scope.modelId == undefined || $scope.modelId == 0) ? '' : $scope.modelId),
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionRatioProductDetail[0].addedProductLists)
                        };
                    }
                    productCommonService.FetchProductNameExceptExistingProduct1(existingProductList, q).then(function (value) {
                        $scope.modalProductNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.modalProductNameJsonData = null;
                    if ($scope.currentEntity)
                        $scope.currentEntity.productId = 0;
                }
            };

            $scope.fillModalProductTextbox = function (obj) {
                if ($scope.currentEntity) {
                    $scope.currentEntity.productId = Number(obj.Value);
                    $scope.currentEntity.productName = obj.Item;
                    GetProductWiseDimension($scope.currentEntity);
                    GetProductWiseUnitType($scope.currentEntity);
                }
                $scope.productName = obj.Item;
                $scope.modalProductNameJsonData = null;
            };
            function SequenceCall(tasks) {
                //Fake a "previous task" for our initial iteration
                var prevPromise;
                var error = new Error();
                angular.forEach(tasks, function (task, key) {
                    var success = task.success || task;
                    var fail = task.fail;
                    var notify = task.notify;
                    var nextPromise;

                    //First task
                    if (!prevPromise) {
                        nextPromise = success();
                        if (!IsPromiseLike(nextPromise)) {
                            error.message = "Task " + key + " did not return a promise.";
                            throw error;
                        }
                    } else {
                        //Wait until the previous promise has resolved or rejected to execute the next task
                        nextPromise = prevPromise.then(
            /*success*/function (data) {
                                if (!success) { return data; }
                                var ret = success(data);
                                if (!IsPromiseLike(ret)) {
                                    error.message = "Task " + key + " did not return a promise.";
                                    throw error;
                                }
                                return ret;
                            },
                            notify);
                    }
                    prevPromise = nextPromise;
                });

                return prevPromise;
            };
            function IsPromiseLike(obj) {
                return obj && angular.isFunction(obj.then);
            };
            var GetProductSubGroupForDropdown = function () {
                var d = $q.defer();

                productSubGroupCommonService.FetchProductSubGroup(
                    '',
                    Number($scope.productGroupId)
                ).then(function (value) {
                    $scope.productSubGroupDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.productSubGroupId = this.Value;
                        }
                    });

                    d.resolve('GetProductSubGroupForDropdown');
                }, function (reason) {
                    console.log(reason);
                });

                return d.promise;
            };

            var GetProductCategoryForDropdown = function () {
                var d = $q.defer();

                productCategoryCommonService.FetchProductCategory(
                    '',
                    Number($scope.productGroupId)
                ).then(function (value) {
                    $scope.productCategoryDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.productCategoryId = this.Value;
                        }
                    });

                    d.resolve('GetProductCategoryForDropdown');
                }, function (reason) {
                    console.log(reason);
                });

                return d.promise;
            };

            var GetProductModelForDropdown = function () {
                var d = $q.defer();

                productModelCommonService.FetchProductModel(
                    $scope.productGroupId, ($scope.productSubGroupDropDownJsonData.length == 0 ? '0' : $scope.productSubGroupId), ($scope.productCategoryDropDownJsonData.length == 0 ? '0' : $scope.productCategoryId), $scope.brandId
                ).then(function (value) {
                    $scope.modelDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.modelId = this.Value;
                        }
                    });

                    d.resolve('GetProductModelForDropdown');
                }, function (reason) {
                    console.log(reason);
                });

                return d.promise;
            }
            // load product group
            var GetProductGroupForDropdown = function () {
                productGroupCommonService.FetchProductGroup(
                    ''
                ).then(function (value) {
                    $scope.productGroupDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.productGroupId = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            var GetBrandForDropdown = function () {
                productBrandCommonService.FetchProductBrand(
                    ''
                ).then(function (value) {
                    $scope.brandDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.brandId = this.Value;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };
            $scope.closeModal = function () {
                $scope.productName = "";
                $scope.productGroupId = "0";
                $scope.productSubGroupId = "0";
                $scope.productCategoryId = "0";
                $scope.brandId = "0";
                $scope.modelId = "0";
            };
            //---------------------End Modal-----------------------
            //add  product
            $scope.clickToAdd = function (entity) {
                if (entity.productId == 0 || (entity.quantity == null || entity.quantity == 0)) {
                    var message = "";
                    if (entity.productId == 0) {
                        message = 'Product can not be Empty.';
                    }
                    else if (entity.quantity == null || entity.quantity == 0) {
                        message = 'Product/Quantity can not be Empty or zero.';
                    }
                    // alert
                    $ngConfirm({
                        title: 'Required',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: message,
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }
                //check If already exist
                var previousIentity = false;
                $.each(entity.addedProductLists, function (index) {
                    if (this.productId === entity.productId && this.productDimensionId == entity.productDimensionId && this.unitTypeId == entity.unitTypeId) {
                        previousIentity = true;
                        return false;
                    }
                });
                if (previousIentity == false) {
                    entity.addedProductLists.push(angular.copy(entity));
                }
                else $.each(entity.addedProductLists, function (index) {
                    if (this.productId === entity.productId && this.productDimensionId == entity.productDimensionId && this.unitTypeId == entity.unitTypeId) {
                        this.quantity = entity.quantity;
                        this.remarks = entity.remarks;
                        return false;
                    }
                });
                $scope.clickToReset(entity);
            }
            //edit Product detail
            $scope.clickToEditProductDetail = function (entity, item) {
                entity.productName = item.productName;
                entity.productId = item.productId;
                entity.productDimensionId = item.productDimensionId;
                entity.dimensionName = item.dimensionName
                entity.unitTypeId = item.unitTypeId;
                entity.unitTypeName = item.unitTypeName;
                entity.quantity = item.quantity;
                entity.remarks = item.remarks;
                GetProductWiseUnitType(entity);
                GetProductWiseDimension(entity);
            };
            $scope.clickRemoveItem = function (entity, item) {
                $.each(entity.addedProductLists, function (index) {
                    if (this.productId === item.productId && this.productDimensionId == item.productDimensionId && this.unitTypeId == item.unitTypeId) {
                        entity.addedProductLists.splice(index, 1);
                        return false;
                    }
                });
            };
            //Reset Product Section
            $scope.clickToReset = function (entity) {
                entity.productName = "";
                entity.productId = 0;
                entity.productDimensionId = null;
                entity.dimensionName = "";
                entity.unitTypeId = 0;
                entity.unitTypeName = "";
                entity.quantity = null;
                entity.remarks = "";
            }
            //edit
            $scope.clickToEdit = function (item) {
                $scope.lblSave = "Update";
                //-----object---------
                $scope.commonSetupConvertionRatio = {};
                $scope.convertionRatioProductDetail[0].addedProductLists.length = 0;
                $scope.convertionRatioProductDetail[1].addedProductLists.length = 0;

                $scope.commonSetupConvertionRatio = new convertionRatioDTO(item);

                $.each($scope.commonSetupConvertionRatio.commonSetupConvertionRatioDetail, function (index) {
                    if (this.productFor == "M")
                        $scope.convertionRatioProductDetail[0].addedProductLists.push(angular.copy(this));
                    else if (this.productFor == "C")
                        $scope.convertionRatioProductDetail[1].addedProductLists.push(angular.copy(this));
                });
            };
            //Save
            $scope.clickSave = function () {
                var productCount = 0;
                var productMessage = 0;
                $scope.commonSetupConvertionRatio.commonSetupConvertionRatioDetail.length = 0;
                $.each($scope.convertionRatioProductDetail, function (index) {
                    if (this.addedProductLists.length == 0) {
                        productCount++;
                        productMessage = this.productForDetail;
                        return;
                    }
                    $.each(this.addedProductLists, function (index2) {
                        $scope.commonSetupConvertionRatio.commonSetupConvertionRatioDetail.push(this);
                    });
                });

                if (productCount > 0) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Have not added any product in ' + productMessage,
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                };
                $scope.isSaved = true;
                convertionRatioSetupService.SaveOrUpdateConvertionRatioSetup($scope.commonSetupConvertionRatio, $scope.lblSave
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.commonSetupConvertionRatio.ratioNo = value.data.Message;
                        // alert
                        $ngConfirm({
                            title: value.data.IsSuccess === true ? 'Success' : 'Failed',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: value.data.Message,
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                    } else {
                        // alert
                        $ngConfirm({
                            title: 'Failed',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: value.data,
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                        $scope.isSaved = false;
                    }

                    if (value.data.IsSuccess) {
                        GetAllConvertionRatioData();
                    }
                });
            };
            $scope.filterConvertionRatioNumber = function () {
                if ($scope.searchConvertionRatioNumber !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllConvertionRatioData();
                }
            };
            $scope.PrintReport = function (ratioNo) {
                var url = applicationBasePath + "/Inventory360Reports/PrintConvertionRatio?id=" + ""
                    + "&no=" + ratioNo
                    + "&user=" + $scope.UserName
                    + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };
            // load all Data lists
            var GetAllConvertionRatioData = function () {
                var q = $scope.searchConvertionRatioNumber === undefined ? '' : $scope.searchConvertionRatioNumber;
                convertionRatioSetupService.FetchConvertionRatioLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allConversionRatioList = value.data;
                    lastPageNo = value.data.LastPageNo;
                    if (value.data.TotalNumberOfRecords > 0) {
                        $scope.showGrid = true;
                        $scope.showRecordsInfo = 'Record(s) showing ' + value.data.Start + " to " + value.data.End + " of " + value.data.TotalNumberOfRecords;
                    } else {
                        $scope.showGrid = false;
                        $scope.showRecordsInfo = "No record(s) found...";
                    }
                }, function (reason) {
                    console.log(reason);
                });
            };
            // initialize
            init();
            GetProductGroupForDropdown();
            GetBrandForDropdown();
            GetAllConvertionRatioData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });