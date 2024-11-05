import "./Card.css"

interface Props {
    companyName: string;
    symbol: string;
    price: number;
}

const Card = ({companyName, symbol, price}: Props) => {
  return<div className="card">
    <img
        src="https://i.imgur.com/xRoQ7Sj.jpeg"
        alt="doge"
    />
    <div className = "details">
        <h2>{companyName} ({symbol})</h2>
        <p>${price}</p>
    </div>
    <p className="Info">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit.
    </p>
    </div>
}

export default Card