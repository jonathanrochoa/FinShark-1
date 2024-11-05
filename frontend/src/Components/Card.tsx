import "./Card.css"

type Props = {}

const Card = (props: Props) => {
  return<div className="card">
    <img
        src="https://i.imgur.com/xRoQ7Sj.jpeg"
        alt="doge"
    />
    <div className = "details">
        <h2>AAPL</h2>
        <p>$110</p>
    </div>
    <p className="Info">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit.
    </p>
    </div>
}

export default Card