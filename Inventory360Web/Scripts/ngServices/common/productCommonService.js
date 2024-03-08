myApp
    .factory('productCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductName = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0035'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchProductName1 = function (
                q, groupId, subGroupId, categoryId,
                brandId, model
            ) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0077'
                        + '?query=' + q
                        + '&groupId=' + groupId
                        + '&subGroupId=' + subGroupId
                        + '&categoryId=' + categoryId
                        + '&brandId=' + brandId
                        + '&model=' + model
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchSaleableProductName = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0061'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchProductWiseUnitType = function (q, productId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0060'
                        + '?query=' + q
                        + '&productId=' + productId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchProductSerialOrNot = function (productId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0074'
                        + '?productId=' + productId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchProductWiseDimension = function (q, productId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0062'
                        + '?query=' + q
                        + '&productId=' + productId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchProductWiseAvailableStockPriceDiscount = function (
                currency, operationTypeId, productId, dimensionId,
                unitTypeId, locationId, wareHouseId
            ) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0064'
                        + '?currency=' + currency
                        + '&operationTypeId=' + operationTypeId
                        + '&productId=' + productId
                        + '&dimensionId=' + dimensionId
                        + '&unitTypeId=' + unitTypeId
                        + '&locationId=' + locationId
                        + '&wareHouseId=' + wareHouseId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchProductCost = function (productId, dimensionId, unitTypeId, locationId, wareHouseId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00218'
                        + '?productId=' + productId
                        + '&dimensionId=' + dimensionId
                        + '&unitTypeId=' + unitTypeId
                        + '&locationId=' + locationId
                        + '&wareHouseId=' + wareHouseId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            // unused code
            fac.FetchProductSerial = function (productId, dimensionId, unitTypeId, locationId, wareHouseId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00219'
                        + '?productId=' + productId
                        + '&dimensionId=' + dimensionId
                        + '&unitTypeId=' + unitTypeId
                        + '&locationId=' + locationId
                        + '&wareHouseId=' + wareHouseId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchProductWiseAvailableStockPriceDiscountByOperationalEvent = function (
                OperationalEvent, OperationalSubEvent, currency, operationTypeId, productId, dimensionId,
                unitTypeId, locationId, wareHouseId
            ) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00220'
                        + '?OperationalEvent=' + OperationalEvent
                        + '&OperationalSubEvent=' + OperationalSubEvent
                        + '&currency=' + currency
                        + '&operationTypeId=' + operationTypeId
                        + '&productId=' + productId
                        + '&dimensionId=' + dimensionId
                        + '&unitTypeId=' + unitTypeId
                        + '&locationId=' + locationId
                        + '&wareHouseId=' + wareHouseId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductBySerial = function (q, ProductId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00223'
                        + '?serial=' + q
                        + '&ProductId=' + ProductId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductByRMASerial = function (q, ProductId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00227'
                        + '?serial=' + q
                        + '&ProductId=' + ProductId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductNameFromRMA = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00228'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GenerateProductSerial = function (productId, serialLength) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00229'
                        + '?ProductId=' + productId
                        + '&serialLength=' + serialLength
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductStockInReferenceInfo = function (productId, Serial, supplierId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00230'
                        + '?productId=' + productId
                        + '&serial=' + Serial
                        + '&supplierId=' + supplierId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductNameExceptExistingProduct = function (entityData, q) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/C00236',
                    dataType: 'json',
                    //data: JSON.stringify({ entityData: entityData, query: q }),
                    data: JSON.stringify(entityData),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };
            fac.FetchProductNameExceptExistingProduct1 = function (entityData, q) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/C00237',
                    dataType: 'json',
                    data: JSON.stringify(entityData),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };

            return fac;
        }
    ]);