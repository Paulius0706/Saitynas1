import React from 'react';
import ValueButton from './ValueButton';
import CostumeTextBox from './CostumeTextBox';
import PlaceHolder from './PlaceHolder';
import CmdButton from './CmdButton';
import Registrations from './Registrations';
import CostumeCheckBox from './CostumeCheckBox';
import CostumeDateTime from './CostumeDateTime';

export default class Times extends React.Component {
    static displayName = Times.name;

    constructor({ pageSize, pageNumber, serviceId }) {
        super({ pageSize, pageNumber, serviceId })
        this.state = { serviceId: serviceId, selected: -1, item: {}, modItem: {}, createItem: {}, itemsList: [], page: pageNumber, size: pageSize, loading: true, edit: false };
        this.pageUp.bind(this)
        this.pageDown.bind(this)

        this.unselectTime.bind(this)
        this.selectTime.bind(this)
    }

    pageUp       = ()   => { this.GetTimes(this.state.serviceId, this.state.page + 1, this.state.size); this.setState({ page: this.state.page + 1, loading: true }); }
    pageDown     = ()   => { this.GetTimes(this.state.serviceId, this.state.page - 1, this.state.size); this.setState({ page: this.state.page - 1, loading: true }); }

    unselectTime =   () => { this.GetTimes(this.state.serviceId, this.state.page, this.state.size); this.setState({ selected: -1, loading: true }); }
    selectTime   = (id) => { this.GetTime (this.state.serviceId, id);                               this.setState({ selected: id, loading: true }); }

    modify = (e) => {
        let modItem = this.state.modItem;
        modItem[e.target.id] = e.target.value;
        let item = this.state.item;
        item[e.target.id] = e.target.value;
        this.setState({ modItem: modItem, item: item });
    }
    addTo = (e) => { let item = this.state.createItem; item[e.target.id] = e.target.value; this.setState({ createItem: item }); }

    editMode = () => { this.setState({ edit: true, modItem: { id: this.state.item.id } }); this.GetTime(this.state.serviceId, this.state.selected); }
    showMode = () => { this.setState({ edit: false }); this.GetTime(this.state.serviceId, this.state.selected); }

    saveChanges = () => {
        this.SaveTime(this.state.serviceId, this.state.modItem);
        this.showMode();
    }

    createTime = (time) => { this.CreateTime(this.state.serviceId, time); }
    deleteTime = (timeId) => { this.setState({ selected: -1, loading: true }); this.DeleteTime(this.state.serviceId, timeId); }


    componentDidMount() {
        if (this.state.selected === -1) this.GetTimes(this.state.serviceId, this.state.page, this.state.size);
        else this.GetTime(this.state.serviceId, this.state.selected);
    }

    RenderTimesListTable(times) {
        let content = (this.state.loading)
            ? <p><em>Loading...</em></p>
            : <table className='w3-table-all'>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Start</th>
                        <th>End</th>
                        <th>Register1</th>
                        <th>Register2</th>
                        <th>SectorInterval 1</th>
                        <th>SectorInterval 2</th>
                        <th>ServiceID</th>
                        <th>Registrations</th>
                    </tr>
                </thead>
                <tbody>
                    {times.map(time =>
                        <tr key={time.id}>
                            <td>{time.id}</td>
                            <td>{time.name}</td>
                            <td>{time.startDate}</td>
                            <td>{time.endDate}</td>
                            <td>{time.register1}</td>
                            <td>{time.register2}</td>
                            <td>{time.sectorInterval1}</td>
                            <td>{time.sectorInterval2}</td>
                            <td>{time.serviceId}</td>
                            <td><ValueButton onClick={this.selectTime} value={time.id} text={'Select'} /><ValueButton onClick={this.deleteTime} value={time.id} text={'Delete'} /></td>
                        </tr>
                    )}
                </tbody>
            </table>
        /*
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string type { get; set; }
        public bool visibility { get; set; }
        public bool register1 { get; set; }
        public bool register2 { get; set; }
        public int maxRegisterTime { get; set; }
        public int sectorInterval1 { get; set; }
        public int sectorInterval2 { get; set; }
        public int serviceId { get; set; }
         */
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
                    <CostumeDateTime id={"startDate"} name={"startDate"} value={this.state.createItem.startDate} onChange={(e) => { this.addTo(e); }} />
                    <CostumeDateTime id={"endDate"} name={"endDate"} value={this.state.createItem.endDate} onChange={(e) => { this.addTo(e); }} />
                    <CostumeCheckBox id={"visibility"} name={"visibility"} value={this.state.createItem.visibility} onChange={(e) => { this.addTo(e); }} />
                    <CostumeCheckBox id={"Registratable 1"} name={"register1"} value={this.state.createItem.register1} onChange={(e) => { this.addTo(e); }} />
                    <CostumeCheckBox id={"Registratable 2"} name={"register2"} value={this.state.createItem.register2} onChange={(e) => { this.addTo(e); }} />
                    <CostumeTextBox id={"Register time"} name={"maxRegisterTime"} value={this.state.createItem.maxRegisterTime} onChange={(e) => { this.addTo(e); }} />
                    <CostumeTextBox id={"Register Interval 1"} name={"sectorInterval1"} value={this.state.createItem.sectorInterval1} onChange={(e) => { this.addTo(e); }} />
                    <CostumeTextBox id={"Register Interval 2"} name={"sectorInterval2"} value={this.state.createItem.sectorInterval2} onChange={(e) => { this.addTo(e); }} />
                    <ValueButton onClick={this.createTime} value={this.state.createItem} text={'Create'} />
                </details>
            </>
        );
    }
    RenderTimeTable(time) {
        // type visibility register1 register2 maxRegisterTime sectorInterval1 sectorInterval2
        //if (this.state.edit) { this.setState({ modItem: { id: time.id } }) }
        let content = (this.state.loading)
            ? <p><em>Loading...</em></p>
            : (this.state.edit)
                ? <div>
                    <CmdButton onClick={this.showMode} text={'Show'} /> <CmdButton onClick={this.saveChanges} text={'Save'} /> <br />
                    <PlaceHolder id={"id"} name={"ID"} value={time.id} />
                    <CostumeTextBox id={"name"} name={"Name"} value={time.name} onChange={(e) => { this.modify(e); }} />
                    <CostumeDateTime id={"startDate"} name={"Start"} value={time.startDate} onChange={(e) => { this.modify(e); }} />
                    <CostumeDateTime id={"endDate"} name={"End"} value={time.endDate} onChange={(e) => { this.modify(e); }} />
                    <CostumeCheckBox id={"register1"} name={"Registratable 1"} value={'' + time.register1} onChange={(e) => { this.modify(e); }} />
                    <CostumeCheckBox id={"register2"} name={"Registratable 2"} value={'' + time.register2} onChange={(e) => { this.modify(e); }} />
                    <CostumeTextBox id={"maxRegisterTime"} name={"Maximum Registrations Times"} value={time.maxRegisterTime} onChange={(e) => { this.modify(e); }} />
                    <CostumeTextBox id={"sectorInterval1"} name={"Interval lenght 1"} value={'' + time.sectorInterval1} onChange={(e) => { this.modify(e); }} />
                    <CostumeTextBox id={"sectorInterval2"} name={"Interval lenght 2"} value={'' + time.sectorInterval2} onChange={(e) => { this.modify(e); }} />
                </div>
                : < div >
                    <CmdButton onClick={this.editMode} text={'Edit'} /> <br />
                    <PlaceHolder name={"ID"} value={time.id} />
                    <PlaceHolder name={"Name"} value={time.name} />
                    <PlaceHolder name={"Start"} value={time.startDate} />
                    <PlaceHolder name={"End"} value={time.endDate} />
                    <PlaceHolder name={"Registratable 1"} value={time.register1} />
                    <PlaceHolder name={"Registratable 2"} value={time.register2} />
                    <PlaceHolder name={"Maximum Registrations Times"} value={time.maxRegisterTime} />
                    <PlaceHolder name={"Interval lenght 1"} value={time.sectorInterval1} />
                    <PlaceHolder name={"Interval lenght 2"} value={time.sectorInterval2} />
                </div>
        return (
            <>
                <CmdButton onClick={this.unselectTime} text={'return'} />
                {content}
                <Registrations pageSize={this.state.size} pageNumber={1} serviceId={this.state.serviceId} timeId={this.state.selected} />
            </>
        );
    }


    render() {
        let contents = this.state.selected === -1
            ? this.RenderTimesListTable(this.state.itemsList)
            : this.RenderTimeTable(this.state.item);
        return (
            <div>
                <h4>Times</h4>
                {contents}
            </div>
        );
    }


    async GetTimes(serviceId, page, size) {
        const response = await fetch('api/services/' + serviceId + '/times?pageNumber=' + page + '&pageSize=' + size);
        const data = await response.json();
        this.setState({ itemsList: data, loading: false });
    }
    async GetTime(serviceId, timeId) {
        const response = await fetch('api/services/' + serviceId + '/times/' + timeId);
        const data = await response.json();
        this.setState({ item: data, loading: false });
    }
    async SaveTime(serviceId, time) {
        for (const [key, value] of Object.entries(this.state.item)) {
            if (!(key in time)) { time[key] = value; }
        }
        time.type = 'null';
        let res = await fetch('api/services/' + serviceId + '/times/' + time.id, {
            method: "PUT",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            }),
            body: JSON.stringify(time)
        });
        await this.GetTime(serviceId, time.id);
    }
    async CreateTime(serviceId, time) {

        time.type = 'null';
        let res = await fetch('api/services/' + serviceId + '/times', {
            method: "POST",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            }),
            body: JSON.stringify(time),
        });
        if (this.state.selected === -1) await this.GetTimes(this.state.serviceId, this.state.page, this.state.size);
    }
    async DeleteTime(serviceId, timeId) {

        let res = await fetch('api/services/' + serviceId + '/times/' + timeId, {
            method: "DELETE",
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem('token')
            })
        });
        if (this.state.selected === -1) await this.GetTimes(this.state.serviceId, this.state.page, this.state.size);
    }

}