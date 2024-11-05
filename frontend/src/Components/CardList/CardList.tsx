import React from 'react'
import Card from '../Card/Card'

interface Props {}

const CardList = (props: Props) => {
  return (
    <div>
        <Card companyName='Apple' symbol='AAPL' price={130}/>
        <Card companyName='Microsoft' symbol='MSFT' price={150}/>
        <Card companyName='Tesla' symbol='TSLA' price={170}/>
    </div>
  )
}

export default CardList