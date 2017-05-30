import React from 'react';
import AddForm from './AddForm';

var hostname = document.location.hostname;
var sitepath = "/";
if (hostname.indexOf("unitec") !== -1) {
        sitepath = "/zhangj188/asp_application1/";
    }

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
                    onMouseEnter={e => this.mouseEnterHandler(e)} 
                    onMouseLeave={e => this.mouseLeaveHandler(e)} 
                    draggable={this.state.unitType !== ''} 
                    onDragStart={e => this.dragStartHandler(e)} 
                    onDragOver={e => this.dragOverHandler(e)} 
                    onDrop={e => this.dropHandler(e)} 
                    onDragEnd={e => this.dragEndHandler(e)} >
                    <div className='map__unit__float-tag map__unit__float-tag--attr'>{floating_tag}</div>
                    {this.state.showForm && this.state.forms}
                    {this.state.hover && this.state.unitType !== '' && <div className='map__unit__float-tag map__unit__float-tag--del' onClick={e => this.onDelete(e)}>X</div>}
               </div>
               {this.state.showForm && <div className='form-popup__background' onClick={e => this.onExitForm(e)} ></div>}
    }

    clickHandler(e){
        if(this.state.unitType === '' && this.state.forms != null){
            this.setState({
                forms: [<AddForm key={0} position={this.props.position} 
                    onExit={(e) => this.onExitForm(e)} 
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

    dragStartHandler(e){
        e.dataTransfer.setData('unit', JSON.stringify(this.state.unit));
        e.dataTransfer.setData('unitType', this.state.unitType);
    }

    dragOverHandler(e){
        if(this.state.unitType === ''){
            e.preventDefault();
        }
    }

    dropHandler(e){
        e.preventDefault();
        var fromUnit = JSON.parse(e.dataTransfer.getData('unit'));
        const fromUnitType = e.dataTransfer.getData('unitType');
        var controllerSlug;
        switch(fromUnitType){
            case 'city': controllerSlug = 'Cities'; break;
            case 'road': controllerSlug = 'Roads'; break;
            case 'pass': controllerSlug = 'Passes'; break;
        }
        $.get(sitepath + controllerSlug + '/EditMove', 
                {fromX: fromUnit.x, 
                 fromY: fromUnit.y, 
                 toX: this.props.position.x, 
                 toY: this.props.position.y} ,
                (data) => {
                    if(data === 'success')  {
                        fromUnit.x = this.props.position.x;
                        fromUnit.y = this.props.position.y;
                        this.setState({
                            unitType: fromUnitType, 
                            unit: fromUnit, 
                            showForm: false, 
                            hover: false
                        });
                    }
                    else{
                        e.dataTransfer.dropEffect = 'none';
                        alert(data);
                    }
                }
        ).promise();//end $.get  
    }

    dragEndHandler(e){
        if(e.dataTransfer.dropEffect !== 'none'){
            this.setState({
                unitType: '', 
                unit: {}, 
                showForm: false, 
                hover: false
            });
        }
    }

    onExitForm(e){
        e.stopPropagation();
        this.setState({
            showForm: false
        });
    }

    onAddCity(city){
        $.get(sitepath + 'Cities/CreateFromGraph', city, (data) => {
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
        $.get(sitepath + 'Roads/CreateFromGraph', road, (data) => {
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
        $.get(sitepath + 'Passes/CreateFromGraph', pass, (data) => {
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

        $.get(sitepath + controllerSlug + '/DeleteFromGraph', {x: this.props.position.x, y: this.props.position.y} ,(data) => {
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

}

export default MapUnit;