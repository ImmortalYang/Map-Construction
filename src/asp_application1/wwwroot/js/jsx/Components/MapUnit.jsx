import React from 'react';
import AddForm from './AddForm';

class MapUnit extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            forms: [], 
            showForm: true
        };
    }

    componentWillReceiveProps(props) {
      this.setState({
        unitType: props.unitType, 
        unit: props.unit
      });  
    }

    render(){
        var className = 'map__unit ';

        if(this.state.unitType !== ''){
            className += 'map__unit--' + this.state.unitType;
            if(this.state.unitType === 'road'){
                switch(this.state.unit.orientation.toString()){
                    case '0': className += '-TopLeft'; break;
                    case '1': className += '-TopRight'; break;
                    case '2': className += '-BottomLeft'; break;
                    case '3': className += '-BottomRight'; break;
                    case '4': className += '-Horizontal'; break;
                    case '5': className += '-Vertical'; break;
                    case '6': className += '-All'; break;
                }
            }
        }
        var floating_tag;
        if(this.state.unitType === 'city'){
            floating_tag = this.state.unit.name;
        }
        else if(this.state.unitType === 'pass'){
            const styleMaxDuration = {width: '50px', height: '10px', backgroundColor: 'rgba(200,200,200,0.8)'};
            const currentDurationWidth = 50/1000*this.state.unit.duration + 'px';
            const styleCurrentDuration = {width: currentDurationWidth, height: '10px', backgroundColor: 'rgba(200,0,0,0.8)'};
            floating_tag = <div style={styleMaxDuration}>
                                <div style={styleCurrentDuration}></div>
                           </div>
        }
        return <div className={className} onClick={e => this.clickHandler(e)} 
                    onContextMenu={e => this.contextMenu(e)} >
                    <span className='map__unit__float-tag'>{floating_tag}</span>
                    {this.state.showForm && this.state.forms}
               </div>
    }

    clickHandler(e){
        if(this.state.unitType === '' && this.state.forms != null){
            this.setState({
                forms: [<AddForm key={0} position={this.props.position} 
                    onCancelAdd={() => this.cancelHandler()} 
                    onCitySubmit={(city) => this.onAddCity(city)} 
                    onRoadSubmit={(road) => this.onAddRoad(road)} 
                    onPassSubmit={(pass) => this.onAddPass(pass)} />], 
                showForm: true
            });
        }
    }

    cancelHandler(){
        this.setState({
            showForm: false
        });
    }

    onAddCity(city){
        $.get('/Cities/CreateFromGraph', city, (data) => {
            if(data === 'success'){
                this.setState({
                    unitType: 'city', 
                    unit: city, 
                    showForm: false
                });
                alert('You have added a city ' + city.name + ' at (' + city.X + ', ' + city.Y + ') successfully.');
            }
            else{
                alert(data);
            }
        });
    }

    onAddRoad(road){
        $.get('/Roads/CreateFromGraph', road, (data) => {
            if(data === 'success'){
                this.setState({
                    unitType: 'road', 
                    unit: road, 
                    showForm: false
                });
                alert('You have added a road at (' + road.X + ', ' + road.Y + ') successfully.');
            }
            else{
                alert(data);
            }
        });
    }

    onAddPass(pass){
        $.get('/Passes/CreateFromGraph', pass, (data) => {
            if(data === 'success'){
                this.setState({
                    unitType: 'pass', 
                    unit: pass, 
                    showForm: false
                });
                alert('You have added a pass at (' + pass.X + ', ' + pass.Y + ') successfully.');
            }
            else{
                alert(data);
            }
        });
    }

    contextMenu(e){
        e.preventDefault();
        
    }
}

export default MapUnit;