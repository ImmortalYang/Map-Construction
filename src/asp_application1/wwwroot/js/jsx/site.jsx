import React from 'react';
import ReactDOM from 'react-dom';
import Map from './Components/Map';

var root = document.getElementById('map-root');
if(root !== null){
    ReactDOM.render(
      <Map url='/MapUnits/GetIndexViewModelJsonAsync' />, 
      document.getElementById('map-root')
    );
}