import React from 'react';
import ValueButton from './ValueButton';
import CmdButton from './CmdButton';
import PlaceHolder from './PlaceHolder';
import CostumeTextBox from './CostumeTextBox';
import CostumeDateTime from './CostumeDateTime';

export default class Registrations extends React.Component {
    static displayName = Registrations.name;

    constructor({ pageSize, pageNumber, serviceId, timeId }) {
        super({ pageSize, pageNumber, serviceId })
        this.state = { serviceId: serviceId, timeId: timeId, selected: -1, item: {}, modItem: {}, createItem: {}, itemsList: [], page: pageNumber, size: pageSize, loading: true, edit: false };
        this.pageUp.bind(this)
        this.pageDown.bind(this)
        this.selectRegistration.bind(this)
    }

    pageUp       = ()   => { this.GetRegistrations(this.state.serviceId, this.state.timeId,     this.state.page + 1, this.state.size); this.setState({ page: this.state.page + 1, loading: true }); }
    pageDown     = ()   => { this.GetRegistrations(this.state.serviceId, this.state.timeId,     this.state.page - 1, this.state.size); this.setState({ page: this.state.page - 1, loading: true }); }

    unselectRegistration =   () => { this.GetRegistrations(this.state.serviceId, this.state.timeId, this.state.page, this.state.size); this.setState({ selected: -1, loading: true }); }
    selectRegistration   = (id) => { this.GetRegistration (this.state.serviceId, this.state.timeId, id);                               this.setState({ selected: id, loading: true }); }

    modify = (e) => {
        let modItem = this.state.modItem;
        modItem[e.target.id] = e.target.value;
        let item = this.state.item;
        item[e.target.id] = e.target.value;
        this.setState({ modItem: modItem, item: item });
    }
    addTo = (e) => { let item = this.state.createItem; item[e.target.id] = e.target.value; this.setState({ createItem: item }); }

    editMode = () => { this.setState({ edit: true, modItem: { id: this.state.item.id } }); this.GetRegistration(this.state.serviceId, this.state.timeId, this.state.selected); }
    showMode = () => { this.setState({ edit: false }); this.GetRegistration(this.state.serviceId, this.state.timeId, this.state.selected); }

    saveChanges = () => {
        this.SaveRegistration(this.state.serviceId, this.state.timeId, this.state.modItem);
        this.showMode();
    }

    createRegistration = (registration) => { this.CreateRegistration(this.state.serviceId, this.state.timeId, registration); }
    deleteRegistration = (registrationId) => { this.setState({ selected: -1, loading: true }); this.DeleteRegistration(this.state.serviceId, this.state.timeId, registrationId); }


    componentDidMount() {
        if (this.state.selected === -1) this.GetRegistrations(this.state.serviceId, this.state.timeId, this.state.page, this.state.size);
        else this.GetRegistration(this.state.serviceId, this.state.timeId, this.state.selected);
    }

    /*
    public int id { get; set; }
        public string userId { get; set; }
        public string type { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string comment { get; set; }
        public int timeId { get; set; }
     */

    RenderRegisterListTable(registrations) {
        let content = (this.state.loading)
            ? <p><em>Loading...</em></p>
            : <table className='w3-table-all'>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>User</th>
                        <th>Type</th>
                        <th>Start</th>
                        <th>End</th>
                        <th>Comment</th>
                        <th>Time</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {registrations.map(registration =>
                        <tr key={registration.id}>
                            <td>{registration.id}</td>
                            <td>{registration.userId}</td>
                            <td>{registration.type}</td>
                            <td>{registration.startDate}</td>
                            <td>{registration.endDate}</td>
                            <td>{registration.comment}</td>
                            <td>{registration.timeId}</td>
                            <td><ValueButton onClick={this.selectRegistration} value={registration.id} text={'Select'} /><ValueButton onClick={this.deleteRegistration} value={registration.id} text={'Delete'} /></td>
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
                    <CostumeTextBox id={"type"} name={"Type"} value={this.state.createItem.type} onChange={(e) => { this.addTo(e); }} />
                    <CostumeDateTime id={"startDate"} name={"Start"} value={this.state.createItem.startDate} onChange={(e) => { this.addTo(e); }} />
                    <CostumeDateTime id={"endDate"} name={"End"} value={this.state.createItem.endDate} onChange={(e) => { this.addTo(e); }} />
                    <CostumeTextBox id={"comment"} name={"Comment"} value={this.state.createItem.comment} onChange={(e) => { this.addTo(e); }} />
                    <ValueButton onClick={this.createRegistration} value={this.state.createItem} text={'Create'} />
                </details>
            </>
        );
    }

    RenderRegisterTable(registration) {
        let content = (this.state.loading)
            ? <p><em>Loading...</em></p>
            : (this.state.edit)
                ? <div>
                    <CmdButton onClick={this.showMode} text={'Show'} /> <CmdButton onClick={this.saveChanges} text={'Save'} /> <br />
                    <PlaceHolder name={"ID"} value={registration.id} />
                    <PlaceHolder name={"Uesr id"} value={registration.userId} />
                    <CostumeTextBox id={"type" } name={"Type"} value={registration.type} onChange={(e) => { this.modify(e); }} />
                    <CostumeDateTime id={"startDate" } name={"Start"} value={registration.startDate} onChange={(e) => { this.modify(e); }} />
                    <CostumeDateTime id={"endDate" } name={"End"} value={registration.endDate} onChange={(e) => { this.modify(e); }} />
                    <CostumeTextBox id={"comment" } name={"Comment"} value={registration.comment} onChange={(e) => { this.modify(e); }} />
                 </div>
                : <div>
                    <CmdButton onClick={this.editMode} text={'Edit'} /> <br />
                    <PlaceHolder name={"ID"} value={registration.id} />
                    <PlaceHolder name={"Uesr id"} value={registration.userId} />
                    <PlaceHolder name={"Type"} value={registration.type} />
                    <PlaceHolder name={"Start"} value={registration.startDate} />
                    <PlaceHolder name={"End"} value={registration.endDate} />
                    <PlaceHolder name={"Comment"} value={registration.comment} />
                 </div>
        return (
            <>
                <CmdButton onClick={this.unselectRegistration} text={'return'} />
                {content}
            </>
        );
    }


    render() {
        let contents = this.state.selected === -1
            ? this.RenderRegisterListTable(this.state.itemsList)
            : this.RenderRegisterTable(this.state.item);
        //let contents = this.RenderServicesListTable(this.state.itemsList, this.state.page)
        return (
            <div>
                <h4>Registrations</h4>
                {contents}
            </div>
        );
    }

    // intercept
    // get post put delete
    // sessionStorage.

    async GetRegistrations(serviceId, timeId, page, size) {
        console.log('api/services/' + serviceId + '/times/' + timeId + '/registrations?pageNumber=' + page + '&pageSize=' + size);
        const response = await fetch('api/services/' + serviceId + '/times/' + timeId + '/registrations?pageNumber=' + page + '&pageSize=' + size);
        const data = await response.json();
        this.setState({ itemsList: data, loading: false });
    }
    async GetRegistration(serviceId, timeId, registrationId) {
        const response = await fetch('api/services/' + serviceId + '/times/' + timeId + '/registrations/' + registrationId);
        const data = await response.json();
        this.setState({ item: data, loading: false });
    }
    async SaveRegistration(serviceId, timeId, registration) {
        for (const [key, value] of Object.entries(this.state.item)) {
            if (!(key in registration)) { registration[key] = value; }
        }
        let res = await fetch('api/services/' + serviceId + '/times/' + timeId + '/registrations/' + registration.id, {
            method: "PUT",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            }),
            body: JSON.stringify(registration)
        });
        await this.GetRegistration(serviceId, timeId, registration.id);
    }
    async CreateRegistration(serviceId, timeId, registration) {
        let res = await fetch('api/services/' + serviceId + '/times/' + timeId + '/registrations', {
            method: "POST",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            }),
            body: JSON.stringify(registration),
        });
        if (this.state.selected === -1) await this.GetRegistrations(this.state.serviceId, this.state.timeId, this.state.page, this.state.size);
    }
    async DeleteRegistration(serviceId, timeId, registrationId) {

        let res = await fetch('api/services/' + serviceId + '/times/' + timeId + '/registrations/' + registrationId, {
            method: "DELETE",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            })
        });
        if (this.state.selected === -1) await this.GetTimes(this.state.serviceId, this.state.page, this.state.size);
    }

}