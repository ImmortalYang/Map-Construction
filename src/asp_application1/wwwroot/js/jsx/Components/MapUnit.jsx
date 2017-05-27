import React from 'react';

class MapUnit extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            unitType: 'empty', 
            unit: {}
        };

    }

    render(){
        var className = 'map__unit ';
        if(this.props.unitType !== undefined){
            className += 'map__unit--' + this.props.unitType;
            if(this.props.unitType === 'road'){
                switch(this.props.unit.orientation){
                    case 0: className += '-TopLeft'; break;
                    case 1: className += '-TopRight'; break;
                    case 2: className += '-BottomLeft'; break;
                    case 3: className += '-BottomRight'; break;
                    case 4: className += '-Horizontal'; break;
                    case 5: className += '-Vertical'; break;
                    case 6: className += '-All'; break;
                }
            }
        }
        return <div className={className}>
               </div>
    }
}

export default MapUnit;