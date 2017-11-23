var app = angular.module("computer", ["ngRoute"])
.config(["$routeProvider",function($routeProvider){
	$routeProvider.
		when("/mostrarClientes",{
			templateUrl: "mostrarClientes.html",
			controller: "AdminCtrl",
			resolve: {
		          dataService: ['ServiceClientes',
		            function(Service) {
		              return Service.getData_clientes().then(function(result) {
		                return {
		                  data: result
		                };
		              });
		            }
		          ]
        	}
		}).
		when("/mostrarEmpleados",{
			templateUrl: "mostrarEmpleados.html",
			controller: "AdminCtrl",
			resolve: {
		          dataService: ['ServiceEmpleados',
		            function(Service) {
		              return Service.getData_empleados().then(function(result) {
		                return {
		                  data: result
		                };
		              });
		            }
		          ]
        	}
		}).
		when("/mostrarProductosSuc",{
			templateUrl: "mostrarMedXSuc.html",
			controller: "AdminCtrl",
			resolve: {
		          dataService: ['ServiceProductosSucursal',
		            function(Service) {
		              return Service.getData_productos().then(function(result) {
		                return {
		                  data: result
		                };
		              });
		            }
		          ]
        	}
		}).
		when("/mostrarProductosComp",{
			templateUrl: "mostrarMedXComp.html",
			controller: "AdminCtrl",
			resolve: {
		          dataService: ['ServiceProductosCompañia',
		            function(Service) {
		              return Service.getData_productos().then(function(result) {
		                return {
		                  data: result
		                };
		              });
		            }
		          ]
        	}
		}).
		when("/mostrarProveedores",{
			templateUrl: "mostrarProveedores.html",
			controller: "AdminCtrl",
			resolve: {
		          dataService: ['ServiceProveedores',
		            function(Service) {
		              return Service.getData_proveedores().then(function(result) {
		                return {
		                  data: result
		                };
		              });
		            }
		          ]
        	}
		}).
		when("/mostrarSucursales",{
			templateUrl: "mostrarSucursales.html",
			controller: "AdminCtrl",
			resolve: {
		          dataService: ['ServiceSucursales',
		            function(Service) {
		              return Service.getData_sucursales().then(function(result) {
		                return {
		                  data: result
		                };
		              });
		            }
		          ]
        	}
		}).
		when("/crearCliente",{
			templateUrl: "crearCliente.html",
			controller: "AdminCtrl"
		}).
		when("/actualizarCliente",{
			templateUrl: "actualizarCliente.html",
			controller: "AdminCtrl"
		}).
		when("/crearEmpleado",{
			templateUrl: "crearEmpleado.html",
			controller: "AdminCtrl"
		}).
		when("/actualizarEmpleado",{
			templateUrl: "actualizarEmpleado.html",
			controller: "AdminCtrl"
		}).
		when("/crearSucursal",{
			templateUrl: "crearSucursal.html",
			controller: "AdminCtrl"
		}).
		when("/actualizarSucursal",{
			templateUrl: "actualizarSucursal.html",
			controller: "AdminCtrl"
		}).
		when("/crearProductoSuc",{
			templateUrl: "crearMedXSuc.html",
			controller: "AdminCtrl"
		}).
		when("/actualizarProductoSuc",{
			templateUrl: "actualizarMedXSuc.html",
			controller: "AdminCtrl"
		}).
		when("/crearProductoComp",{
			templateUrl: "crearMedXComp.html",
			controller: "AdminCtrl"
		}).
		when("/actualizarProductoComp",{
			templateUrl: "actualizarMedXComp.html",
			controller: "AdminCtrl"
		}).
		when("/crearProveedor",{
			templateUrl: "crearProveedor.html",
			controller: "AdminCtrl"
		}).
		when("/actualizarProveedor",{
			templateUrl: "actualizarProveedor.html",
			controller: "AdminCtrl"
		}).
		when("/mostrarPerfil",{
			templateUrl: "perfil.html",
			controller: "AdminCtrl"
		}).
		when("/seleccionarSucursal",{
			templateUrl: "seleccionarSucursal.html",
			controller: "AdminCtrl"
		});
	}])

.controller("AdminCtrl",["$scope", "$http", "dataService", function($scope, $http, dataService){
	document.getElementById("result").innerHTML =localStorage.nombreCompania;
	$scope.perfil = localStorage.perfil;
	for(i in $scope.perfil){
        $scope.perfil = $scope.perfil.replace("%7B","{");
		$scope.perfil = $scope.perfil.replace("%22","\"");
		$scope.perfil = $scope.perfil.replace("%7D","}");
    };        	
    $scope.perfil = JSON.parse($scope.perfil);
    console.log($scope.perfil);
	$scope.row = {};
	$scope.data = dataService;
	$scope.pruebas = $scope.data.data;
	$scope.isNumberKey = function(){
		var charCode = (evt.which) ? evt.which : event.keyCode
   		if (charCode > 31 && (charCode < 48 || charCode > 57)) {
       		alert("Solo se permiten numeros")
        		return false;
    	}
    	return true;
	};

	$scope.cargaSucursales = function(){
		$scope.sucursales = {};
		var first = {};
		$http.get("http://webapi220171117104514.azurewebsites.net/api/sucursal?x=1").
        	then(function(response) {
            	$scope.sucs = response.data.Sucursales;
        	});
	}

	$scope.cargaProveedores = function(){
		$scope.proveedores = {};
		var first = {};
		$http.get("http://webapi220171117104514.azurewebsites.net/api/proveedor").
        	then(function(response) {
            	$scope.proveedores = response.data.Proveedores;
        	});
	}

	$scope.sucursalRedirect = function(){
		var skillsSelect = document.getElementById("sucursal");
		var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
        var y = 0;
        for(var i in $scope.sucs){
           	if($scope.sucs[i].Nombre == selectedText){
           		y = $scope.sucs[i].ID;
           	}
  			console.log(y);
		};
		localStorage.sucursal = y;
		window.location.href = "#!mostrarProductosSuc";
	}

	//////////////////////CLIENTES/////////////////////////////////////

		$scope.creaCliente = function(){
			var today = $scope.fecha;
			today = today.toLocaleFormat('%d-%b-%Y');
			str = today.toString();
			str = str.replace("ene.","01");
			str = str.replace("feb.","02");
			str = str.replace("mar.","03");
			str = str.replace("abr.","04");
			str = str.replace("may.","05");
			str = str.replace("jun.","06");
			str = str.replace("jul.","07");
			str = str.replace("ago.","08");
			str = str.replace("set.","09");
			str = str.replace("oct.","10");
			str = str.replace("nov.","11");
			str = str.replace("dic.","12");
			str = str.substring(3,5) + "-" + str.substring(0,2) + "-" + str.substring(6,10);
			console.log(str);
			var Data ={"cedula":$scope.cedula,
				"fNacimiento":str,
				"nombre1":$scope.nombre1,
				"nombre2":$scope.nombre2,
				"apellido1":$scope.apellido1,
				"apellido2":$scope.apellido2,
				"provincia":$scope.provincia,
				"canton":$scope.canton,
				"distrito":$scope.distrito,
				"indicaciones":$scope.direccion,
				"telefono":$scope.telefono}; 
			console.log(Data);
        	$http.post("http://webapi220171117104514.azurewebsites.net/api/cliente?codigo=2", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarClientes";
        	});

        	
        };

        $scope.crearClienteRedirect = function(){
            window.location.href = "#!crearCliente";
        };

        $scope.actualizaCliente = function(){
        	console.log("Entro");
			var Data ={"cedula":$scope.row.cedula,
				"fNacimiento":$scope.fecha,
				"nombre1":$scope.nombre1,
				"nombre2":$scope.nombre2,
				"apellido1":$scope.apellido1,
				"apellido2":$scope.apellido2,
				"provincia":$scope.provincia,
				"canton":$scope.canton,
				"distrito":$scope.distrito,
				"indicaciones":$scope.direccion,
				"telefono":$scope.telefono}; 
			console.log(Data);
        	$http.put("http://webapi220171117104514.azurewebsites.net/api/cliente", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarClientes";
        	});

        	
        };

        $scope.actualizarClienteRedirect = function(){
        	var res = JSON.stringify($scope.row);
            window.location.href = "#!actualizarCliente?" + res;
        };

        $scope.selectClientRow = function(){
			var table = document.getElementById('table');
                
                for(var i = 1; i < table.rows.length; i++)
                {
                    table.rows[i].onclick = function()
                    {
                    	var str = this.cells[5].innerHTML;
                    	str = str.substring(0,10);
                    	str = str.substring(5,10) + "-" + str.substring(0,4);
                    	$scope.row = {"cedula":parseInt(this.cells[4].innerHTML, 10),
							"fNacimiento":str,
							"nombre1":this.cells[0].innerHTML,
							"nombre2":this.cells[1].innerHTML,
							"apellido1":this.cells[2].innerHTML,
							"apellido2":this.cells[3].innerHTML,
							"provincia":this.cells[6].innerHTML,
							"canton":this.cells[7].innerHTML,
							"distrito":this.cells[8].innerHTML,
							"indicaciones":this.cells[9].innerHTML,
							"telefono":parseInt(this.cells[10].innerHTML, 10)}; 
                    };
                }
        };

        $scope.datosCliente = function(){
        	$scope.data = window.location.href.toString().split("?")[1];
        	for(i in $scope.data){
        		$scope.data = $scope.data.replace("%7B","{");
				$scope.data = $scope.data.replace("%22","\"");
				$scope.data = $scope.data.replace("%7D","}");
        	};        	
        	console.log($scope.data);
        	$scope.data = JSON.parse($scope.data);
        	for(i in $scope.data.indicaciones){
        		$scope.data.indicaciones = $scope.data.indicaciones.replace("%20"," ");
        	};
        	$scope.row = $scope.data;
        	$scope.nombre1 = $scope.data.nombre1;
        	$scope.nombre2 = $scope.data.nombre2;
        	$scope.apellido1 = $scope.data.apellido1;
        	$scope.apellido2 = $scope.data.apellido2;
        	$scope.telefono = $scope.data.telefono;
        	$scope.provincia = $scope.data.provincia;
        	$scope.canton = $scope.data.canton;
        	$scope.distrito = $scope.data.distrito;
        	$scope.direccion = $scope.data.indicaciones;
        	$scope.fecha = $scope.data.fNacimiento;
        };

        $scope.eliminaCliente = function(){
        	var Data = {"cedula":$scope.row.cedula};
        	console.log(Data);
        	$http.delete("http://webapi220171117104514.azurewebsites.net/api/cliente?cedula="+$scope.row.cedula.toString()).
        	then(function(response) {
        		console.log("Entro");
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarClientes";
        	});
        };

        ///////////////////////////////////////////////////////////////////////////////

        /////////////////////////////EMPLEADOS///////////////////////////////////////////////

        $scope.creaEmpleado = function(){
			var today = $scope.fecha;
			today = today.toLocaleFormat('%d-%b-%Y');
			str = today.toString();
			str = str.replace("ene.","01");
			str = str.replace("feb.","02");
			str = str.replace("mar.","03");
			str = str.replace("abr.","04");
			str = str.replace("may.","05");
			str = str.replace("jun.","06");
			str = str.replace("jul.","07");
			str = str.replace("ago.","08");
			str = str.replace("set.","09");
			str = str.replace("oct.","10");
			str = str.replace("nov.","11");
			str = str.replace("dic.","12");
			str = str.substring(3,5) + "-" + str.substring(0,2) + "-" + str.substring(6,10);
			console.log(str);
			var skillsSelect = document.getElementById("sucursal");
			var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
            var y = 0;
            for(var i in $scope.sucs){
            	if($scope.sucs[i].Nombre == selectedText){
            		y = $scope.sucs[i].ID;
            	}
       			console.log(y);
		    };
			var Data = {"cedula":$scope.cedula,
				"fNacimiento":str,
				"contrasena":$scope.contrasena,
				"nombre1":$scope.nombre1,
				"nombre2":$scope.nombre2,
				"apellido1":$scope.apellido1,
				"apellido2":$scope.apellido2,
				"provincia":$scope.provincia,
				"canton":$scope.canton,
				"distrito":$scope.distrito,
				"indicaciones":$scope.direccion,
				"telefono":$scope.telefono,
				"idsucursal":y,
				"idcompania":1};
			console.log(Data);
        	$http.post("http://webapi220171117104514.azurewebsites.net/api/empleado?codigo=3", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarEmpleados";
        	});

        	
        };

        $scope.actualizaEmpleado = function(){
        	console.log("Entro");
        	var skillsSelect = document.getElementById("sucursal");
			var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
            var y = 0;
            for(var i in $scope.sucs){
            	if($scope.sucs[i].Nombre == selectedText){
            		y = $scope.sucs[i].ID;
            	}
       			console.log(y);
		    };
			var Data ={"cedula":$scope.row.cedula,
				"fNacimiento":$scope.fecha,
				"contrasena":$scope.contrasena,
				"nombre1":$scope.nombre1,
				"nombre2":$scope.nombre2,
				"apellido1":$scope.apellido1,
				"apellido2":$scope.apellido2,
				"provincia":$scope.provincia,
				"canton":$scope.canton,
				"distrito":$scope.distrito,
				"indicaciones":$scope.direccion,
				"telefono":$scope.telefono,
				"idsucursal":y}; 
			console.log(Data);
        	$http.put("http://webapi220171117104514.azurewebsites.net/api/empleado", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarEmpleados";
        	});

        	
        };

        $scope.crearEmpleadoRedirect = function(){
            window.location.href = "#!crearEmpleado";
        };

        $scope.actualizarEmpleadoRedirect = function(){
        	var res = JSON.stringify($scope.row);
            window.location.href = "#!actualizarEmpleado?" + res;
        };

        $scope.selectEmpRow = function(){
			var table = document.getElementById('table');
                
                for(var i = 1; i < table.rows.length; i++)
                {
                    table.rows[i].onclick = function()
                    {
                    	var str = this.cells[8].innerHTML;
                    	str = str.substring(0,10);
                    	str = str.substring(5,10) + "-" + str.substring(0,4);
                    	$scope.row = {"cedula":parseInt(this.cells[7].innerHTML, 10),
							"fNacimiento":str,
							"contrasena":this.cells[0].innerHTML,
							"nombre1":this.cells[3].innerHTML,
							"nombre2":this.cells[4].innerHTML,
							"apellido1":this.cells[5].innerHTML,
							"apellido2":this.cells[6].innerHTML,
							"provincia":this.cells[9].innerHTML,
							"canton":this.cells[10].innerHTML,
							"distrito":this.cells[11].innerHTML,
							"indicaciones":this.cells[12].innerHTML,
							"telefono":parseInt(this.cells[13].innerHTML, 10),
							"idsucursal":this.cells[1].innerHTML}; 
                    };
                }
        };

        $scope.datosEmpleado = function(){
        	$scope.cargaSucursales();
        	$scope.data = window.location.href.toString().split("?")[1];
        	console.log($scope.data);
        	for(i in $scope.data){
        		$scope.data = $scope.data.replace("%7B","{");
				$scope.data = $scope.data.replace("%22","\"");
				$scope.data = $scope.data.replace("%7D","}");
        	};        	
        	console.log($scope.data);
        	$scope.data = JSON.parse($scope.data);
        	for(i in $scope.data.indicaciones){
        		$scope.data.indicaciones = $scope.data.indicaciones.replace("%20"," ");
        	};
        	$scope.row = $scope.data;
        	$scope.nombre1 = $scope.data.nombre1;
        	$scope.nombre2 = $scope.data.nombre2;
        	$scope.apellido1 = $scope.data.apellido1;
        	$scope.apellido2 = $scope.data.apellido2;
        	$scope.contrasena = $scope.data.contrasena;
        	$scope.telefono = $scope.data.telefono;
        	$scope.provincia = $scope.data.provincia;
        	$scope.canton = $scope.data.canton;
        	$scope.distrito = $scope.data.distrito;
        	$scope.direccion = $scope.data.indicaciones;
        	$scope.fecha = $scope.data.fNacimiento;
        	$scope.sucursal = 1;
        };

        $scope.eliminaEmpleado = function(){
        	var Data = {"cedula":$scope.row.cedula};
        	console.log(Data);
        	$http.delete("http://webapi220171117104514.azurewebsites.net/api/empleado?cedula="+$scope.row.cedula.toString()).
        	then(function(response) {
        		console.log("Entro");
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarEmpleados";
        	});
        };

        /////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////SUCURSALES////////////////////////////////////////

        $scope.creaSucursal = function(){
			var Data = {"idcompania":$scope.id,
				"nombre":$scope.nombre,
				"provincia":$scope.provincia,
				"canton":$scope.canton,
				"distrito":$scope.distrito,
				"descripcion":$scope.descripcion,
				"indicaciones":$scope.direccion};
			console.log(Data);
        	$http.post("http://webapi220171117104514.azurewebsites.net/api/sucursal?codigo=2", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarSucursales";
        	});

        	
        }; 

        $scope.actualizaSucursal = function(){
        	console.log("Entro");
			var Data ={"id":$scope.row.id,
				"idcompania":$scope.id,
				"cedadmin":$scope.row.cedadmin,
				"nombre":$scope.nombre,
				"provincia":$scope.provincia,
				"canton":$scope.canton,
				"distrito":$scope.distrito,
				"descripcion":$scope.descripcion,
				"indicaciones":$scope.direccion}; 
			console.log(Data);
        	$http.put("http://webapi220171117104514.azurewebsites.net/api/sucursal", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarSucursales";
        	});

        	
        };

        $scope.crearSucursalRedirect = function(){
            window.location.href = "#!crearSucursal";
        };  

        $scope.actualizarSucursalRedirect = function(){
        	var res = JSON.stringify($scope.row);
            window.location.href = "#!actualizarSucursal?" + res;
        };

        $scope.selectSucRow = function(){
			var table = document.getElementById('table');
                
                for(var i = 1; i < table.rows.length; i++)
                {
                    table.rows[i].onclick = function()
                    {
                    	$scope.row = {"id":parseInt(this.cells[0].innerHTML,10),
							"idcompania":parseInt(this.cells[2].innerHTML,10),
							"cedadmin":parseInt(this.cells[1].innerHTML,10),
							 "nombre": this.cells[3].innerHTML,
							 "provincia": this.cells[5].innerHTML,
							 "canton" : this.cells[6].innerHTML,
							 "distrito":this.cells[7].innerHTML,
							 "descripcion":this.cells[4].innerHTML,
							 "indicaciones":this.cells[8].innerHTML
							}; 
						var x = "nombre";
						console.log($scope.row);
						console.log($scope.row[x]);
                    };
                }
        };

        $scope.datosSucursal = function(){
        	$scope.data = window.location.href.toString().split("?")[1];
        	console.log($scope.data);
        	for(i in $scope.data){
        		$scope.data = $scope.data.replace("%7B","{");
				$scope.data = $scope.data.replace("%22","\"");
				$scope.data = $scope.data.replace("%7D","}");
        	};        	
        	console.log($scope.data);
        	$scope.data = JSON.parse($scope.data);
        	for(i in $scope.data.indicaciones){
        		$scope.data.indicaciones = $scope.data.indicaciones.replace("%20"," ");
        	};
        	$scope.row = $scope.data;
        	$scope.nombre = $scope.data.nombre;
        	$scope.id = $scope.data.idcompania;
        	$scope.descripcion = $scope.data.descripcion;
        	$scope.provincia = $scope.data.provincia;
        	$scope.canton = $scope.data.canton;
        	$scope.distrito = $scope.data.distrito;
        	$scope.direccion = $scope.data.indicaciones;
        	$scope.sucursal = 1;
        };

        $scope.eliminaSucursal = function(){
        	$http.delete("http://webapi220171117104514.azurewebsites.net/api/sucursal?id="+$scope.row.id.toString()).
        	then(function(response) {
        		console.log("Entro");
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarSucursales";
        	});
        };

        ////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////MEDICAMENTOS POR COMPAÑIA//////////////////////////////

        $scope.creaProductoCompania = function(){
        	var skillsSelect = document.getElementById("proveedor");
			var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
            var y = 0;
            for(var i in $scope.proveedores){
            	if($scope.proveedores[i].nombre == selectedText){
            		y = $scope.proveedores[i].id;
            	}
       			console.log(y);
		    };
			var Data = {"idcompania":parseInt(localStorage.compania,10),
						 "ean": $scope.ean,
						 "casafarmaceutica": $scope.casa,
						 "idproveedor" : y
						};
			console.log(Data);
        	$http.post("http://webapi220171117104514.azurewebsites.net/api/medicamento?codigo=1", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProductosComp";
        	});

        	
        }; 

        $scope.actualizaProductoCompania = function(){
        	console.log("Entro");
        	var skillsSelect = document.getElementById("proveedor");
			var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
            var y = 0;
			for(var i in $scope.proveedores){
            	if($scope.proveedores[i].nombre == selectedText){
            		y = $scope.proveedores[i].id;
            	}
       			console.log(y);
		    };
			var Data = {"idproveedor" : y,
						"idmedicamento" : $scope.row.idmedicamento
						};
			console.log(Data);
        	$http.put("http://webapi220171117104514.azurewebsites.net/api/medicamento?codigo=2", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProductosComp";
        	});

        	
        };

        $scope.crearProductoCompaniaRedirect = function(){
            window.location.href = "#!crearProductoComp";
        };  

        $scope.actualizarProductoCompaniaRedirect = function(){
        	var res = JSON.stringify($scope.row);
            window.location.href = "#!actualizarProductoComp?" + res;
        };

        $scope.selectProdCompRow = function(){
			var table = document.getElementById('table');
                
                for(var i = 1; i < table.rows.length; i++)
                {
                    table.rows[i].onclick = function()
                    {
                    	$scope.row = {"idmedicamento":this.cells[0].innerHTML,
							"nombre":this.cells[1].innerHTML,
							"casafarmaceutica":this.cells[2].innerHTML,
							"cantidadtotal":parseInt(this.cells[3].innerHTML,10),
							}; 
                    };
                }
        };

        $scope.datosProductoCompania = function(){
        	$scope.cargaProveedores();
        	$scope.data = window.location.href.toString().split("?")[1];
        	console.log($scope.data);
        	for(i in $scope.data){
        		$scope.data = $scope.data.replace("%7B","{");
				$scope.data = $scope.data.replace("%22","\"");
				$scope.data = $scope.data.replace("%7D","}");
        	};        	
        	console.log($scope.data);
        	$scope.data = JSON.parse($scope.data);
        	$scope.row = $scope.data;
        };

        $scope.eliminaProductoCompania = function(){
        	$http.delete("http://webapi220171117104514.azurewebsites.net/api/medicamento?codigo=2&idmed="+$scope.row.idmedicamento+"&idsuc="+$scope.perfil.AdmSucursalID.toString()).
        	then(function(response) {
        		console.log($scope.row.idmedicamento);
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProductosComp";
        	});
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////MEDICAMENTOS POR SUCURSAL/////////////////////////////////////

        $scope.creaProductoSucursal = function(){
			var Data = {"idsucursal":parseInt(localStorage.sucursal,10),
				"idmedicamento":$scope.id,
				"cantidad":$scope.cantidad,
				"precio":$scope.precio};
			console.log(Data);
        	$http.post("http://webapi220171117104514.azurewebsites.net/api/medicamento?codigo=2", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProductosSuc";
        	});

        	
        }; 

        $scope.actualizaProductoSucursal = function(){
			var Data = {"idmedicamento":$scope.id,
				"idsucursal":parseInt(localStorage.sucursal,10),	
				"cantidad":$scope.cantidad,
				"precio":$scope.precio};
			console.log("Actualizar");
			console.log(Data);
        	$http.put("http://webapi220171117104514.azurewebsites.net/api/medicamento?codigo=1", Data).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProductosSuc";
        	});

        	
        };

        $scope.crearProductoSucursalRedirect = function(){
            window.location.href = "#!crearProductoSuc";
        };  

        $scope.actualizarProductoSucursalRedirect = function(){
        	var res = JSON.stringify($scope.row);
            window.location.href = "#!actualizarProductoSuc?" + res;
        };

        $scope.selectProdSucRow = function(){
			var table = document.getElementById('table');
                
                for(var i = 1; i < table.rows.length; i++)
                {
                    table.rows[i].onclick = function()
                    {
                    	$scope.row = {"idmedicamento":this.cells[0].innerHTML,
							"nombre":this.cells[1].innerHTML,
							"cantidad":parseInt(this.cells[2].innerHTML,10),
							 "precio":parseInt(this.cells[3].innerHTML,10),
							}; 
                    };
                }
        };

        $scope.datosProductoSucursal = function(){
        	$scope.cargaSucursales();
        	$scope.data = window.location.href.toString().split("?")[1];
        	console.log($scope.data);
        	for(i in $scope.data){
        		$scope.data = $scope.data.replace("%7B","{");
				$scope.data = $scope.data.replace("%22","\"");
				$scope.data = $scope.data.replace("%7D","}");
        	};        	
        	console.log($scope.data);
        	$scope.data = JSON.parse($scope.data);
        	$scope.row = $scope.data;
        	$scope.id = $scope.data.idmedicamento;
        	$scope.cantidad = $scope.data.cantidad;
        	$scope.precio = $scope.data.precio;
        };

        $scope.eliminaProductoSucursal = function(){
        	$http.delete("http://webapi220171117104514.azurewebsites.net/api/medicamento?codigo=1&idmed="+$scope.row.idmedicamento+"&idsuc="+localStorage.sucursal.toString()).
        	then(function(response) {
        		console.log($scope.row.idmedicamento);
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProductosSuc";
        	});
        };

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////PROVEEDORES///////////////////////////////////////////////////

        $scope.creaProveedor = function(){
        	$http.post("http://webapi220171117104514.azurewebsites.net/api/proveedor?nombre="+$scope.nombre).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProveedores";
        	});

        	
        }; 

        $scope.actualizaProveedor = function(){
        	$http.put("http://webapi220171117104514.azurewebsites.net/api/proveedor?id="+$scope.row.id+"&nombre="+$scope.nombre).
        	then(function(response) {
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProveedores";
        	});

        	
        };

        $scope.crearProveedorRedirect = function(){
            window.location.href = "#!crearProveedor";
        };  

        $scope.actualizarProveedorRedirect = function(){
        	var res = JSON.stringify($scope.row);
            window.location.href = "#!actualizarProveedor?" + res;
        };

        $scope.selectProvRow = function(){
			var table = document.getElementById('table');
                
                for(var i = 1; i < table.rows.length; i++)
                {
                    table.rows[i].onclick = function()
                    {
                    	$scope.row = {"id":parseInt(this.cells[0].innerHTML,10),
							"nombre":this.cells[1].innerHTML,
							}; 
                    };
                }
        };

        $scope.datosProveedor = function(){
        	$scope.data = window.location.href.toString().split("?")[1];
        	console.log($scope.data);
        	for(i in $scope.data){
        		$scope.data = $scope.data.replace("%7B","{");
				$scope.data = $scope.data.replace("%22","\"");
				$scope.data = $scope.data.replace("%7D","}");
        	};        	
        	console.log($scope.data);
        	$scope.data = JSON.parse($scope.data);
        	$scope.row = $scope.data;
        	$scope.nombre = $scope.data.nombre;
        };

        $scope.eliminaProveedor = function(){
        	$http.delete("http://webapi220171117104514.azurewebsites.net/api/proveedor?id="+$scope.row.id).
        	then(function(response) {
        		console.log($scope.row.idmedicamento);
            	$scope.res = response.data;
            	console.log($scope.res)
            	window.location.href = "#!mostrarProductosSuc";
        	});
        };

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
       
        
}])
.service('ServiceClientes', ['$timeout', "$http",
    function($timeout,$http) {
      return {
        getData_clientes: function() {
          //return a test promise
          return $timeout(function() {
            return $http.get("http://webapi220171117104514.azurewebsites.net/api/cliente").
        	then(function(response) {
            	return response.data.Clientes;
        	});;
          }, 1000);
        }
      };
    }
  ])
.service('ServiceEmpleados', ['$timeout', "$http",
    function($timeout,$http) {
      return {
        getData_empleados: function() {
          //return a test promise
          return $timeout(function() {
            return $http.get("http://webapi220171117104514.azurewebsites.net/api/empleado?x="+localStorage.compania.toString()).
        	then(function(response) {
            	return response.data.Empleados;
        	});;
          }, 1000);
        }
      };
    }
  ])
.service('ServiceProductosSucursal', ['$timeout', "$http",
    function($timeout,$http) {
      return {
        getData_productos: function() {
          //return a test promise
          return $timeout(function() {
            return $http.get("http://webapi220171117104514.azurewebsites.net/api/medicamento?id="+localStorage.sucursal.toString()+"&codigo=1").
        	then(function(response) {
            	return response.data.Medicamentos;
        	});;
          }, 1000);
        }
      };
    }
  ])
.service('ServiceProductosCompañia', ['$timeout', "$http",
    function($timeout,$http) {
      return {
        getData_productos: function() {
          //return a test promise
          return $timeout(function() {
            return $http.get("http://webapi220171117104514.azurewebsites.net/api/medicamento?id="+localStorage.sucursal.toString()+"&codigo=2").
        	then(function(response) {
            	return response.data.Medicamentos;
        	});;
          }, 1000);
        }
      };
    }
  ])
.service('ServiceProveedores', ['$timeout', "$http",
    function($timeout,$http) {
      return {
        getData_proveedores: function() {
          //return a test promise
          return $timeout(function() {
            return $http.get("http://webapi220171117104514.azurewebsites.net/api/proveedor").
        	then(function(response) {
            	return response.data.Proveedores;
        	});;
          }, 1000);
        }
      };
    }
  ])
.service('ServiceSucursales', ['$timeout', "$http",
    function($timeout,$http) {
      return {
        getData_sucursales: function() {
          //return a test promise
          return $timeout(function() {
            return $http.get("http://webapi220171117104514.azurewebsites.net/api/sucursal?x="+localStorage.compania.toString()).
        	then(function(response) {
            	return response.data.Sucursales;
        	});;
          }, 1000);
        }
      };
    }
  ])
.service('dataService', function(ServiceClientes, ServiceEmpleados, ServiceProductosSucursal, ServiceProductosCompañia, ServiceProveedores, ServiceSucursales) {
    var _result = {
      data: null
    };
    ServiceClientes.getData_clientes().then(function(result) {
      _result.data = result;
      return result;
    });
    ServiceEmpleados.getData_empleados().then(function(result) {
      _result.data = result;
      return result;
    });
    ServiceProductosSucursal.getData_productos().then(function(result) {
      _result.data = result;
      return result;
    });
    ServiceProductosCompañia.getData_productos().then(function(result) {
      _result.data = result;
      return result;
    });
    ServiceProveedores.getData_proveedores().then(function(result) {
      _result.data = result;
      return result;
    });
    ServiceSucursales.getData_sucursales().then(function(result) {
      _result.data = result;
      return result;
    });
    return _result;
  });