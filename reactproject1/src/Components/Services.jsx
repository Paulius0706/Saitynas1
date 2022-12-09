import React from 'react';
import CmdButton from './CmdButton';
import CostumeTextBox from './CostumeTextBox';
import PlaceHolder from './PlaceHolder';
import Times from './Times';
import ValueButton from './ValueButton';

export default class Services extends React.Component {
    static displayName = Services.name;

    constructor({ pageSize, pageNumber }) {
        super({ pageSize, pageNumber })
        this.state = { selected: -1, item: {}, createItem: {}, itemsList: [], page: pageNumber, size: pageSize, loading: true, edit: false };
        this.pageUp.bind(this)
        this.pageDown.bind(this)

        this.selectService.bind(this)
        this.unselectService.bind(this)

        this.editMode.bind(this);
        this.showMode.bind(this);

        this.modify.bind(this);
        this.addTo.bind(this);

        this.saveChanges.bind(this);

        this.createService.bind(this);
        this.deleteService.bind(this);
    }

    pageUp   = () => { this.GetServices(this.state.page + 1, this.state.size); this.setState({ page: this.state.page + 1, loading: true }); }
    pageDown = () => { this.GetServices(this.state.page - 1, this.state.size); this.setState({ page: this.state.page - 1, loading: true }); }

    selectService = (id) => { this.GetService(id); this.setState({ selected: id, loading: true }); }
    unselectService = () => { this.GetServices(this.state.page, this.state.size); this.setState({ selected: -1, loading: true }); }

    modify = (e) => { let item = this.state.item; item[e.target.id] = e.target.value; this.setState({ item: item }); }
    addTo  = (e) => { let item = this.state.createItem; item[e.target.id] = e.target.value; this.setState({ createItem: item }); }

    editMode = () => { this.setState({ edit: true }); this.GetService(this.state.selected); }
    showMode = () => { this.setState({ edit: false }); this.GetService(this.state.selected); }

    saveChanges = () => { this.SaveService(this.state.item); this.GetService(this.state.item.id); this.showMode(); }

    createService = (service) => { this.CreateService(service); }
    deleteService = (serviceId) => { this.setState({ selected: -1, loading: true }); this.DeleteService(serviceId); }


    componentDidMount() {
        if (this.state.selected === -1) this.GetServices(this.state.page, this.state.size);
        else this.GetService(this.state.selected);
    }

    RenderServicesListTable(services) {
        let content = (this.state.loading)
            ? <p><em>Loading...</em></p>
            : <table className='w3-table-all' >
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>User</th>
                        <th>Name</th>
                        <th>Active</th>
                        <th>Public</th>
                        <th>Description</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {services.map(service =>
                        <tr key={service.id}>
                            <td>{service.id}</td>
                            <td>{service.userId}</td>
                            <td>{service.name}</td>
                            <td>{'' + service.isActive}</td>
                            <td>{'' + service.isPublic}</td>
                            <td>{service.description}</td>
                            <td><ValueButton onClick={this.selectService} value={service.id} text={'Select'} /><ValueButton onClick={this.deleteService} value={service.id} text={'Delete'} /></td>
                        </tr>
                    )}
                </tbody>
            </table>

        return (
            <>
                <CmdButton onClick={this.pageDown} text={"last"} />
                <button>{this.state.page}</button>
                <CmdButton onClick={this.pageUp} text={"next"} />
                {content}
                <CmdButton onClick={this.pageDown} text={"last"} />
                <button>{this.state.page}</button>
                <CmdButton onClick={this.pageUp} text={"next"} />
                <details>
                    <summary>Create</summary>
                    <CostumeTextBox id={"name"} name={"Name"} value={this.state.createItem.name} onChange={(e) => { this.addTo(e); }} />
                    <CostumeTextBox id={"description"} name={"Description"} value={this.state.createItem.description} onChange={(e) => { this.addTo(e); }} />
                    <ValueButton onClick={this.createService} value={this.state.createItem} text={'Create'} />
                </details>
            </>
        );
    }

    //// id, name, value, type, show
    RenderServiceTable(service) {
        //<CostumeTextBox id={"id"} value={service.id} onChange={(e) => { this.modify(e); }} />
        let content = (this.state.loading)
            ? <p><em>Loading...</em></p>
            : (this.state.edit)
                ? <div>
                    <CmdButton onClick={this.showMode} text={'Show'} /> <CmdButton onClick={this.saveChanges} text={'Save'} /> <br />
                    <PlaceHolder id={"id"} name={"ID"} value={service.id} />
                    <PlaceHolder id={"userId"} name={"User Id"} value={service.userId} />
                    <PlaceHolder id={"name"} name={"Name"} value={service.name} />
                    <CostumeTextBox id={"isActive"} name={"Is Active"} value={'' + service.isActive} onChange={(e) => { this.modify(e); }} />
                    <CostumeTextBox id={"isPublic"} name={"Is Public"} value={'' + service.isPublic} onChange={(e) => { this.modify(e); }} />
                    <CostumeTextBox id={"description"} name={"Description"} value={service.description} onChange={(e) => { this.modify(e); }} />
                </div>
                : <div>
                    <CmdButton onClick={this.editMode} text={'Edit'} /> <br />
                    <PlaceHolder name={"ID"} value={service.id}/>
                    <PlaceHolder name={"User Id"} value={service.userId}/>
                    <PlaceHolder name={"Name"} value={service.name}/>
                    <PlaceHolder name={"Is Active"} value={'' + service.isActive}/>
                    <PlaceHolder name={"Is Public"} value={'' + service.isPublic}/>
                    <PlaceHolder name={"Description"} value={service.description}/>
                </div>
        return (
            <>
                <CmdButton onClick={this.unselectService} text={'return'} />
                {content}
                <Times pageSize={this.state.size} pageNumber={1} serviceId={this.state.selected} />
            </>
        );
    }


    render() {
        let contents = this.state.selected === -1
            ? this.RenderServicesListTable(this.state.itemsList)
            : this.RenderServiceTable(this.state.item);
        return (
            <div>
                <h4>Services</h4> 
                {contents}
                
            </div>
        );
    }

    // intercept
    // get post put delete
    // sessionStorage.

    async GetServices(page, size) {
        const response = await fetch('api/services?pageNumber=' + page + '&pageSize=' + size);
        const data = await response.json();
        this.setState({ itemsList: data, loading: false });
    }
    async GetService(serviceId) {
        const response = await fetch('api/services/' + serviceId);
        const data = await response.json();
        this.setState({ item: data, loading: false });
    }
    async SaveService(service) {

        let res = await fetch('api/services/' + service.id, {
            method: "PUT",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            }),
            body: JSON.stringify({
                description: service.description,
                isActive: ('true' === service.isActive),
                isPublic: ('true' === service.isPublic),
            }),
        });
    }
    async CreateService(service) {

        let res = await fetch('api/services', {
            method: "POST",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            }),
            body: JSON.stringify(service),
        });
        if (this.state.selected === -1) await this.GetServices(this.state.page, this.state.size);
    }
    async DeleteService(serviceId) {

        let res = await fetch('api/services/' + serviceId, {
            method: "DELETE",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            })
        });
        if (this.state.selected === -1) await this.GetServices(this.state.page, this.state.size);
    }

}