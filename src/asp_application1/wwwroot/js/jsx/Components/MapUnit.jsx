import React from 'react';
import AddForm from './AddForm';

class MapUnit extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            forms: [], 
            showForm: true, 
            hover: false
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
                    onContextMenu={e => this.contextMenu(e)} 
                    onMouseEnter={e => this.mouseEnterHandler(e)} 
                    onMouseLeave={e => this.mouseLeaveHandler(e)} >
                    <div className='map__unit__float-tag'>{floating_tag}</div>
                    {this.state.showForm && this.state.forms}
                    {this.state.hover && this.state.unitType !== '' && <div className='form-popup form-popup--delete' onClick={e => this.onDelete(e)}>X</div>}
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

    mouseEnterHandler(e){
        this.setState({ hover: true });
    }

    mouseLeaveHandler(e){
        this.setState({ hover: false });
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
                    showForm: false, 
                    hover: false
                });
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
                    showForm: false, 
                    hover: false
                });
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
                    showForm: false, 
                    hover: false
                });
            }
            else{
                alert(data);
            }
        });
    }

    onDelete(e){
        e.stopPropagation();
        var controllerSlug;
        switch(this.state.unitType){
            case 'city': controllerSlug = 'Cities'; break;
            case 'road': controllerSlug = 'Roads'; break;
            case 'pass': controllerSlug = 'Passes'; break;
        }

        $.get('/' + controllerSlug + '/DeleteFromGraph', {x: this.state.unit.x, y: this.state.unit.y} ,(data) => {
            if(data === 'success'){
                this.setState({
                    unitType: '', 
                    unit: {}, 
                    showForm: false, 
                    hover: false
                });
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