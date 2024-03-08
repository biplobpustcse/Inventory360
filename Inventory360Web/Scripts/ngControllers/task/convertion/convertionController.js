myApp
    .controller('convertionController', ['$scope', '$q', '$filter', '$ngConfirm', 'dataTableRows', 'productGroupCommonService', 'convertionService',
        'productSubGroupCommonService', 'productCategoryCommonService', 'productBrandCommonService', 'productModelCommonService',
        'productCommonService', 'locationCommonService', 'userService', 'productWiseSerialCommonService', 'applicationBasePath',
        function (
            $scope, $q, $filter, $ngConfirm, dataTableRows, productGroupCommonService, convertionService,
            productSubGroupCommonService, productCategoryCommonService, productBrandCommonService, productModelCommonService,
            productCommonService, locationCommonService, userService, productWiseSerialCommonService, applicationBasePath
        ) {
            $scope.saveMode = true;
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;


            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllConvertionData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllConvertionData();
                }
            };
            //variable
            $scope.convertionProductDetail = [];
            $scope.lblSave = "Save";
            //-----object---------
            $scope.commonTaskConvertion = {};
            function convertionDTO(defaltData) {
                this.convertionId = defaltData.convertionId || 0;
                this.convertionNo = defaltData.convertionNo || "";
                this.convertionDate = $filter('date')(defaltData.convertionDate, "M/d/yyyy") || $filter('date')(new Date(), "M/d/yyyy");
                this.remarks = defaltData.remarks || "";
                this.convertionType = defaltData.convertionType || "";
                this.convertionFor = defaltData.convertionFor || "";
                this.convertionRatioId = defaltData.convertionRatioId || 0;
                this.approved = defaltData.approved || "N";
                this.approvedBy = defaltData.approvedBy || null;
                this.approvedDate = defaltData.approvedDate || null;
                this.cancelReason = defaltData.cancelReason || "";
                this.commonTaskConvertionDetail = defaltData.commonTaskConvertionDetail || [];
                //extra
                this.numberOfConversionUnit = defaltData.numberOfConversionUnit || null;
                this.ratioNo = defaltData.ratioNo || "";
                this.ratioTitle = defaltData.ratioTitle || "";
                this.description = defaltData.description || "";
                this.remarks = defaltData.remarks || "";
            }
            function convertionDetailDTO(defaltData) {
                this.ConvertionDetailId = 0;
                this.ConvertionId = 0;
                this.productFor = defaltData.productFor || "";
                this.productId = 0;
                this.unitTypeId = 0;
                this.primaryUnitTypeId = 0;
                this.secondaryUnitTypeId = 0;
                this.tertiaryUnitTypeId = 0;
                this.secondaryConversionRatio = 0;
                this.tertiaryConversionRatio = 0;
                this.productDimensionId = null;
                this.wareHouseId = 0;
                this.quantity = null;
                this.cost = 0;
                this.cost1 = 0;
                this.cost2 = 0;     
                this.commonTaskConvertionDetailSerial = defaltData.commonTaskConvertionDetailSerial || [];
                //extra
                this.totalProductValue = defaltData.totalProductValue || null;
                this.unitTypeName = "";
                this.productForDetail = defaltData.productForDetail || "";
                this.productName = "";
                this.dimensionName = "";
                this.wareHouseName = 0;
                this.serialAvailable = false;
                this.productNameJsonData = null;
                this.addedProductLists = [];
            }
            function init() {
                //-----object---------
                $scope.commonTaskConvertion = new convertionDTO({ convertionType: "R", convertionFor: "A", approved: "N" });

                var convertionRatioDetailMObj = new convertionDetailDTO({ productFor: "M", productForDetail: "Main Product" });
                $scope.convertionProductDetail.push(convertionRatioDetailMObj);
                var convertionRatioDetailCObj = new convertionDetailDTO({ productFor: "C", productForDetail: "Component Product" });
                $scope.convertionProductDetail.push(convertionRatioDetailCObj);
            }
            function initWarehouseByProductDetail() {
                $.each($scope.convertionProductDetail, function (index) {
                    GetWarehouseByProductDetail(this);
                });
            }
            //change Conversion Against Ratio
            $scope.changeConversionAgainstRatio = function () {
                $scope.commonTaskConvertion.convertionRatioId = 0;
                $scope.commonTaskConvertion.ratioNo = "";
                $scope.commonTaskConvertion.ratioTitle = "";
                $scope.commonTaskConvertion.description = "";
                $scope.convertionProductDetail[0].addedProductLists.length = 0;
                $scope.convertionProductDetail[1].addedProductLists.length = 0;
            };
            //
            $scope.changeNumberOfConversionUnit = function () {
                $.each($scope.convertionProductDetail, function (index) {
                    $.each(this.addedProductLists, function (index2) {
                        if (!this.previousQuantity) {
                            this.previousQuantity = angular.copy(this.quantity);
                        }
                        if ($scope.commonTaskConvertion.numberOfConversionUnit && parseFloat($scope.commonTaskConvertion.numberOfConversionUnit) > 1) {
                            this.quantity = this.previousQuantity * parseFloat($scope.commonTaskConvertion.numberOfConversionUnit);
                            this.totalProductValue = this.previousQuantity * parseFloat(this.cost) * parseFloat($scope.commonTaskConvertion.numberOfConversionUnit);
                        } else {
                            this.quantity = this.previousQuantity;
                            this.totalProductValue = this.previousQuantity * parseFloat(this.cost);
                        }
                    });
                });
            };
            //
            $scope.changeCost = function (item) {
                if (item.cost) {
                    item.totalProductValue = parseFloat(item.cost) * parseFloat(item.quantity);
                } else {
                    item.totalProductValue = 0;
                }
            };
            //
            //Get Convertion Ratio
            var GetConvertionRatio = function () {
                convertionService.FetchConvertionRatio().then(function (value) {
                    $scope.convertionRatioDropDownJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            };
            $scope.getConvertionInfo = function () {
                var convertionRatioId = $scope.commonTaskConvertion.convertionRatioId;
                convertionService.FetchConvertionRatioById(convertionRatioId).then(function (value) {
                    var item = value.data;
                    //-----object---------                    
                    $scope.convertionProductDetail[0].addedProductLists.length = 0;
                    $scope.convertionProductDetail[1].addedProductLists.length = 0;

                    $scope.commonTaskConvertion.convertionRatioId = item.convertionRatioId;
                    $scope.commonTaskConvertion.ratioNo = item.ratioNo;
                    $scope.commonTaskConvertion.ratioTitle = item.ratioTitle;
                    $scope.commonTaskConvertion.description = item.description;

                    $.each(item.commonSetupConvertionRatioDetail, function (index, item) {

                        item.commonTaskConvertionDetailSerial = [];                        
                        item.wareHouseId = '0';
                        item.cost = 0;
                        var promise = GetProductCost(item);
                        promise.then(function () {
                            item.primaryUnitTypeId = item.unitTypeId;
                            item.totalCost = item.cost * item.quantity;
                            item.totalProductValue = item.cost * item.quantity;                            
                            GetWarehouseByProductDetail(item);
                            if (item.productFor == "M")
                                $scope.convertionProductDetail[0].addedProductLists.push(angular.copy(item));
                            else if (item.productFor == "C")
                                $scope.convertionProductDetail[1].addedProductLists.push(angular.copy(item));
                        });
                    });

                }, function (reason) {
                    console.log(reason);
                });                
            };
            //Get Warehouse
            var GetWarehouse = function () {
                locationCommonService.FetchWarehouseByLoggedLocationAndCompany().then(function (value) {
                    $scope.warehouseDropDownJsonData = value.data;
                    initWarehouseByProductDetail();
                }, function (reason) {
                    console.log(reason);
                });
            };
            var GetWarehouseByProductDetail = function (entity) {
                entity.warehouseDropDownJsonData = $scope.warehouseDropDownJsonData;
            };
            //get dimension
            $scope.getWarehouseName = function (item) {
                $.each(item.warehouseDropDownJsonData, function () {
                    if (this.Value === item.wareHouseId) {
                        item.wareHouseName = this.Item;
                    }
                });
            };
            //get product cost
            var GetProductCost = function (item) {
                var deferred = $q.defer();
                productCommonService.FetchProductCost(item.productId, item.productDimensionId == null ? 0 : item.productDimensionId,
                    item.unitTypeId, 0, item.wareHouseId
                ).then(function (value) {
                    if (value.data) {
                        item.cost = value.data.Cost;
                    }
                    else {
                        item.cost = 0;
                    }
                    deferred.resolve(item);
                }, function (reason) {
                    console.log(reason);
                });
                return deferred.promise;
            };
            //detail section Product
            $scope.getProductName = function (entity) {
                var q = (entity.productName === undefined || entity.productName == "*") ? '' : entity.productName;
                if (q.length >= 2 || entity.productName == "*") {
                    //when Main product then Except Component product Vice versa
                    var existingProductList = {};
                    if (entity.productFor == "M") {
                        existingProductList = {
                            query: q,
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionProductDetail[1].addedProductLists)
                        };
                    }
                    else if (entity.productFor == "C") {
                        existingProductList = {
                            query: q,
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionProductDetail[0].addedProductLists)
                        };
                    }
                    productCommonService.FetchProductNameExceptExistingProduct(existingProductList, q).then(function (value) {
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
                entity.primaryUnitTypeId = obj.PrimaryUnitTypeId;
                entity.secondaryUnitTypeId = obj.SecondaryUnitTypeId;
                entity.tertiaryUnitTypeId = obj.tertiaryUnitTypeId;
                entity.secondaryConversionRatio = obj.SecondaryConversionRatio;
                entity.tertiaryConversionRatio = obj.TertiaryConversionRatio;
                GetProductWiseDimension(entity);
                var promiseUnitType = GetProductWiseUnitType(entity);
                promiseUnitType.then(function () {
                    var promise = GetProductCost(entity);
                    promise.then(function () {
                    });
                    var promiseSerialOrNot = ProductSerialOrNot(entity);
                    promiseSerialOrNot.then(function () {
                        GetProductWiseSerial(entity);
                    });
                });
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
                        var mainProduct = angular.copy($scope.convertionProductDetail[1].addedProductLists);
                        $.each(mainProduct, function () {
                            if (this.productId == entity.productId) {
                                tmpProductDimensionIds.push(this.productDimensionId);
                            }
                        });
                    }
                    else if (entity.productFor == "C") {
                        var componentProduct = angular.copy($scope.convertionProductDetail[0].addedProductLists);
                        $.each(componentProduct, function () {
                            if (this.productId == entity.productId) {
                                tmpProductDimensionIds.push(this.productDimensionId);
                            }
                        });
                    }
                    if (tmpProductDimensionIds.length > 0) {
                        var filterData = value.data.filter(x => !tmpProductDimensionIds.includes(x.Value))
                        entity.productDimensionDropDownJsonData = filterData;
                        if (filterData.length == 0) {
                            $scope.clickToReset(entity);
                        }
                    }
                    else {
                        entity.productDimensionDropDownJsonData = value.data;
                    }
                    if (entity.productDimensionDropDownJsonData.length > 0) {
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
                var deferred = $q.defer();
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
                    deferred.resolve(entity);
                }, function (reason) {
                    console.log(reason);
                });
                return deferred.promise;
            };
            //get unit type
            $scope.getUnitTypeName = function (entity) {
                $.each(entity.unitTypeDropDownJsonData, function () {
                    if (this.Value === entity.unitTypeId) {
                        entity.unitTypeName = this.Item;
                    }
                });
            };
            var ProductSerialOrNot = function (entity) {
                var deferred = $q.defer();
                productCommonService.FetchProductSerialOrNot(entity.productId).then(function (value) {
                    entity.serialAvailable = value.data;
                    deferred.resolve(entity);
                }, function (reason) {
                    console.log(reason);
                });
                return deferred.promise;
            };
            //GetProductWiseSerial
            var GetProductWiseSerial = function (entity) {
                if (entity.serialAvailable) {
                    entity.searchRecords = undefined;
                    entity.qtyOfSerial = undefined;
                    entity.serialRadio = 'N';
                    entity.isNormalSerialSelection = true;

                    productWiseSerialCommonService.FetchAllSerialByProduct('', entity.productId, (entity.productDimensionId == null ? '0' : entity.productDimensionId.toString()),
                        (entity.unitTypeId == undefined ? '0' : entity.unitTypeId), entity.wareHouseId ? '0' : entity.wareHouseId
                    ).then(function (value) {
                        entity.productWiseSerialJsonData = value.data;
                        entity.availableStock = entity.productWiseSerialJsonData.length;
                        CountCheckedSerial(entity);
                    }, function (reason) {
                        console.log(reason);
                    });
                }
            };
            //clickSerialRadio
            $scope.clickSerialRadio = function (entity) {
                entity.searchRecords = undefined;
                entity.qtyOfSerial = undefined;
                entity.checkAll = false;
                $.each(entity.productWiseSerialJsonData, function () {
                    this.IsSelected = false;
                });

                var serialSelectionType = entity.serialRadio;
                if (serialSelectionType === 'N') {
                    entity.isNormalSerialSelection = true;
                }
                else {
                    entity.isNormalSerialSelection = false;
                }

                CountCheckedSerial(entity);
            };
            //
            var CountCheckedSerial = function (entity) {
                var checkedQty = entity.productWiseSerialJsonData.filter(x => x.IsSelected).length;
                entity.quantity = checkedQty;
                entity.totalSerialSelected = checkedQty;
                entity.givenQuantity = checkedQty;
            }
            $scope.clickCheckedAll = function (entity) {
                entity.addedSerialLists = [];
                $.each(entity.productWiseSerialJsonData, function () {
                    this.IsSelected = entity.checkAll;
                    entity.addedSerialLists.push({
                        "Serial": this.Serial,
                        "AdditionalSerial": this.AdditionalSerial
                    });
                });

                CountCheckedSerial(entity);
            };

            $scope.clickIndChecked = function (entity) {
                entity.addedSerialLists = [];
                $.each(entity.productWiseSerialJsonData, function () {
                    if (this.IsSelected) {
                        entity.addedSerialLists.push({
                            "Serial": this.Serial,
                            "AdditionalSerial": this.AdditionalSerial
                        });
                    }
                });

                CountCheckedSerial(entity);
            };
            $scope.checkedQtyWiseSerial = function (entity) {
                var availableStock = parseFloat(entity.availableStock);
                var maxQty = parseFloat(entity.qtyOfSerial == undefined || entity.qtyOfSerial == '' ? '0' : entity.qtyOfSerial);
                if (maxQty > availableStock) {
                    entity.qtyOfSerial = undefined;
                    maxQty = 0;
                }

                entity.addedSerialLists = [];
                // first set IsSelected = false
                $.each(entity.productWiseSerialJsonData, function () {
                    this.IsSelected = false;
                });

                $.each(entity.productWiseSerialJsonData, function () {
                    if (maxQty == 0) return;

                    this.IsSelected = true;
                    entity.addedSerialLists.push({
                        "Serial": this.Serial,
                        "AdditionalSerial": this.AdditionalSerial
                    });
                    --maxQty;
                });

                CountCheckedSerial(entity);
            };
            //.......................



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
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionProductDetail[1].addedProductLists)
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
                            CommonSetupProductSearchDetail: angular.copy($scope.convertionProductDetail[0].addedProductLists)
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
                    var promiseUnitType = GetProductWiseUnitType($scope.currentEntity);
                    promiseUnitType.then(function () {
                        var promise = GetProductCost($scope.currentEntity);
                        promise.then(function () {
                        });
                        var promiseSerialOrNot = ProductSerialOrNot($scope.currentEntity);
                        promiseSerialOrNot.then(function () {
                            GetProductWiseSerial($scope.currentEntity);
                        });
                    });
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
                var reset = true;
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
                $scope.changeCost(entity);
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
                        if ($scope.commonTaskConvertion.convertionType == 'R') {
                            if (this.quantity != entity.quantity) {
                                reset = false;
                                // alert
                                $ngConfirm({
                                    title: 'Required',
                                    icon: 'glyphicon glyphicon-exclamation-sign',
                                    theme: 'supervan',
                                    content: 'Serial Selection must be equal to Quantity',
                                    animation: 'scale',
                                    buttons: {
                                        Ok: {
                                            btnClass: "btn-blue"
                                        }
                                    },
                                });
                                return;
                            }
                            else {
                                //this.addedSerialLists = entity.addedSerialLists;
                                this.addedSerialLists = angular.copy(entity.addedSerialLists);
                            }
                        }
                        this.quantity = entity.quantity;
                        this.cost = entity.cost;
                        this.totalProductValue = entity.totalProductValue;
                        this.wareHouseId = entity.wareHouseId;
                        return false;
                    }
                });
                if (reset) {
                    $scope.clickToReset(entity);
                }
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
                entity.cost = item.cost;
                entity.totalProductValue = item.totalProductValue;
                entity.remarks = item.remarks;
                entity.serialAvailable = item.serialAvailable;
                if ($scope.commonTaskConvertion.convertionType == 'R') {
                    GetProductWiseSerial(entity);
                }
                else {
                    GetProductWiseDimension(entity);
                    GetProductWiseUnitType(entity);
                }
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
                entity.cost = 0;
                entity.totalProductValue = 0;
                entity.remarks = "";
                entity.serialAvailable = false;
                entity.addedSerialLists.length = 0;
            }
            //Save
            $scope.clickSave = function () {
                var productCount = 0;
                var productMessage = "";
                $scope.commonTaskConvertion.commonTaskConvertionDetail.length = 0;
                $.each($scope.convertionProductDetail, function (index) {
                    if (this.addedProductLists.length == 0) {
                        productCount++;
                        productMessage = 'Have not added any product in ' + this.productForDetail;
                        return;
                    }
                    $.each(this.addedProductLists, function (index2) {
                        var convertionDetail = this;

                        if (convertionDetail.serialAvailable) {
                            if (convertionDetail.addedSerialLists == undefined || convertionDetail.addedSerialLists.length == 0) {
                                productCount++;
                                productMessage = "No serial added for " + "product " + convertionDetail.productName + " in " + convertionDetail.productForDetail;
                                return;
                            }
                        }
                        if (convertionDetail.addedSerialLists) {
                            $.each(convertionDetail.addedSerialLists, function (index3) {
                                convertionDetail.commonTaskConvertionDetailSerial.push(this);
                            });
                        }
                        $scope.commonTaskConvertion.commonTaskConvertionDetail.push(convertionDetail);
                    });
                });

                if (productCount > 0) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: productMessage,
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
                convertionService.SaveConvertion($scope.commonTaskConvertion
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.commonTaskConvertion.convertionNo = value.data.Message;
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
                        GetAllConvertionData();
                    }
                });
            };
            $scope.filterConvertionNumber = function () {
                if ($scope.searchConvertionNumber !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllConvertionData();
                }
            };
            $scope.PrintReport = function (convertionNo) {
                var url = applicationBasePath + "/Inventory360Reports/PrintConvertion?id=" + ""
                    + "&no=" + convertionNo
                    + "&user=" + $scope.UserName
                    + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };
            $scope.getUnitCostTotal = function (addedProductLists) {
                var total = 0;
                for (var i = 0; i < addedProductLists.length; i++) {
                    if (addedProductLists[i].cost) {
                        total += parseFloat(addedProductLists[i].cost);
                    }
                }
                return parseFloat(total).toFixed(2);
            }
            $scope.getTotal = function (addedProductLists) {
                var total = 0;
                for (var i = 0; i < addedProductLists.length; i++) {
                    if (addedProductLists[i].totalProductValue) {
                        total += addedProductLists[i].totalProductValue;
                    }
                }
                return parseFloat(total).toFixed(2);
            }

            // load all Data lists
            var GetAllConvertionData = function () {
                var q = $scope.searchConvertionNumber === undefined ? '' : $scope.searchConvertionNumber;
                convertionService.FetchConvertionLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allConversionList = value.data;
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
            GetWarehouse();
            GetConvertionRatio();
            GetProductGroupForDropdown();
            GetBrandForDropdown();
            GetAllConvertionData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });