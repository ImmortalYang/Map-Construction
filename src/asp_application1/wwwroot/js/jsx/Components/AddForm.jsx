import React from 'react';

class AddForm extends React.Component{
    constructor(props){
        super(props);  
        this.state = {
            name: '', 
            orientation: 0, 
            duration: 1000
        }; 
    }

    render(){
        return (<div className='form-popup'>
                    <form className='form' onSubmit={e => this.handleCitySubmit(e)}>
                        <div className='form-group'>
                            <label htmlFor='Name'>Name: </label>
                            <input className='form-control' type='text' name='Name' 
                                    onChange={e => this.handleNameChange(e)} />
                        </div> 
                        <div className='form-group'>
                            <input className='form-control' type='submit' value='Add City' />
                        </div>
                    </form>
                    <form className='form' onSubmit={e => this.handleRoadSubmit(e)}>
                        <div className='form-group'>
                            <label htmlFor='Orientation'>Orientation: </label>
                            <select className='form-control' name='Orientation' 
                                    onChange={e => this.handleOrientationChange(e)} >
                                <option value='0'>TopLeft</option>
                                <option value='1'>TopRight</option>
                                <option value='2'>BottomLeft</option>
                                <option value='3'>BottomRight</option>
                                <option value='4'>Horizontal</option>
                                <option value='5'>Vertical</option>
                                <option value='6'>All</option>
                            </select>
                        </div> 
                        <div className='form-group'>
                            <input className='form-control' type='submit' value='Add Road' />
                        </div>
                    </form>
                    <form className='form' onSubmit={e => this.handlePassSubmit(e)}>
                        <div className='form-group'>
                            <label htmlFor='Duration'>Duration: </label>
                            <input className='form-control' type='number' name='Duration' 
                                    onChange={e => this.handleDurationChange(e)} />
                        </div> 
                        <div className='form-group'>
                            <input className='form-control' type='submit' value='Add Pass' />
                        </div>
                    </form>
                    <input className='form-control' type='button' value='Cancel' onClick={e => this.cancelClickHandler(e)}/>
                </div>);
    }

    cancelClickHandler(e){
        this.props.onExit(e);
    }

    handleNameChange(e){
        this.setState({
            name: e.target.value
        });
    }

    handleOrientationChange(e){
        this.setState({
            orientation: e.target.value
        });
    }

    handleDurationChange(e){
        this.setState({
            duration: e.target.value
        });
    }

    handleCitySubmit(e){
        e.stopPropagation();
        e.preventDefault();
        this.props.onCitySubmit({
            x: this.props.position.x, 
            y: this.props.position.y, 
            name: this.state.name
        });
    }

    handleRoadSubmit(e){
        e.stopPropagation();
        e.preventDefault();
        this.props.onRoadSubmit({
            x: this.props.position.x, 
            y: this.props.position.y, 
            orientation: this.state.orientation
        });
    }

    handlePassSubmit(e){
        e.stopPropagation();
        e.preventDefault();
        this.props.onPassSubmit({
            x: this.props.position.x, 
            y: this.props.position.y, 
            duration: this.state.duration
        });
    }
}

export default AddForm;