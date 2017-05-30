import React from 'react';
import ReactDOM from 'react-dom';
import Map from './Components/Map';

var hostname = document.location.hostname;
var sitepath = "/";
if (hostname.indexOf("unitec") !== -1) {
        sitepath = "/zhangj188/asp_application1/";
    }
var root = document.getElementById('map-root');
if(root !== null){
    ReactDOM.render(
      <Map url= {sitepath + 'MapUnits/GetIndexViewModelJsonAsync'} />, 
      document.getElementById('map-root')
    );
}