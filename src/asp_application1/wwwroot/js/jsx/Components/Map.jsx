import React from 'react';
import $ from 'jquery';
import MapUnit from './MapUnit';

class Map extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            units: []
        };
        for(var row = 0; row < 20; row++){
            this.state.units[row] = [];
            for(var col = 0; col < 20; col++){
                this.state.units[row].push({
                    unitType: '', 
                    unit: {}, 
                    position: {
                        x: col, 
                        y: row
                    }
                });
            }
        }
        $.get(this.props.url, data => this.loadUnitsFromData(data));
    }

    render(){
        var mapUnits = this.state.units.map(row => 
                    row.map(item => 
                    <MapUnit unitType={item.unitType} unit={item.unit} position={item.position} />)
                );
        return <div className='map'>
                    {mapUnits}
               </div>
    }

    loadUnitsFromData(dataModel){
        $.each(dataModel.cities, (index, city) => {
            this.state.units[city.y-1][city.x-1] = {
                unitType: 'city', 
                unit: city
            };    
        });
        $.each(dataModel.passes, (index, pass) => {
            this.state.units[pass.y-1][pass.x-1] = {
                unitType: 'pass', 
                unit: pass
            };  
        });
        $.each(dataModel.roads, (index, road) => {
            this.state.units[road.y-1][road.x-1] = {
                unitType: 'road', 
                unit: road
            };  
        });
        this.setState({
            units: this.state.units
        });
    }

}

export default Map;